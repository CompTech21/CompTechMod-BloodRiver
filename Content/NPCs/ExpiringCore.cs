using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Projectiles;
using CompTechMod.Content.Items;
using CompTechMod.Common.Systems;
using System;

namespace CompTechMod.Content.NPCs
{
    [AutoloadBossHead]
    public class ExpiringCore : ModNPC
    {
        private int phase;
        private int attackTimer;
        private float rotationAngle;
        private Vector2? teleportPos;
        private int teleportTimer;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 70;
            NPC.damage = 150;
            NPC.defense = 30;
            NPC.lifeMax = 350000;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            Music = MusicID.Boss5;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest();
                return;
            }

            // Биом-чек
            NPC.dontTakeDamage = !player.ZoneCrimson;

            // Пульсирующее свечение
            float pulse = (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.5f + 0.5f;
            Lighting.AddLight(NPC.Center, 1.2f * pulse, 0f, 0f);

            attackTimer++;
            rotationAngle += 0.03f;

            float hp = (float)NPC.life / NPC.lifeMax;
            phase = hp switch
            {
                <= 0.25f => 4,
                <= 0.5f => 3,
                <= 0.75f => 2,
                _ => 1
            };

            switch (phase)
            {
                case 1:
                    PhaseOne(player);
                    break;
                case 2:
                    PhaseTwo(player);
                    break;
                case 3:
                    PhaseThree(player);
                    break;
                case 4:
                    PhaseFour(player);
                    break;
            }
        }

        // ================= ФАЗЫ =================

        private void PhaseOne(Player player)
        {
            Hover(player, 240, 4f);

            if (attackTimer % 90 == 0)
                RadialShot(6, 7f);
        }

        private void PhaseTwo(Player player)
        {
            Hover(player, 180, 5f);

            if (attackTimer % 120 == 0)
                SpiralShot(16, 8f);

            if (attackTimer % 240 == 0)
                DashWithWarning(player);
        }

        private void PhaseThree(Player player)
        {
            Orbit(player, 260, 0.03f);

            if (attackTimer % 80 == 0)
                RadialShot(10, 8f);

            if (attackTimer % 200 == 0)
                Teleport(player);
        }

        private void PhaseFour(Player player)
        {
            Orbit(player, 200, 0.06f);

            if (attackTimer % 40 == 0)
                SpiralShot(20, 9f);

            if (attackTimer % 120 == 0)
                DashWithWarning(player);
        }

        // ================= ДВИЖЕНИЕ =================

        private void Hover(Player player, float height, float speed)
        {
            Vector2 target = player.Center + new Vector2(0, -height);
            NPC.velocity = Vector2.Lerp(NPC.velocity, (target - NPC.Center) * 0.05f, 0.08f);
        }

        private void Orbit(Player player, float radius, float speed)
        {
            Vector2 orbitPos = player.Center +
                new Vector2((float)Math.Cos(rotationAngle), (float)Math.Sin(rotationAngle)) * radius;

            NPC.velocity = (orbitPos - NPC.Center) * speed;
        }

        // ================= АТАКИ =================

        private void RadialShot(int count, float speed)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 vel = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / count * i) * speed;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel,
                    ModContent.ProjectileType<BloodBlast>(), NPC.damage / 4, 2f);
            }

            SpawnPulseDust();
        }

        private void SpiralShot(int count, float speed)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 vel = Vector2.UnitX
                    .RotatedBy(rotationAngle + MathHelper.TwoPi / count * i) * speed;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel,
                    ModContent.ProjectileType<BloodBlast>(), NPC.damage / 4, 2f);
            }

            SpawnPulseDust();
        }

        private void DashWithWarning(Player player)
        {
            // Предупреждение
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDustPerfect(NPC.Center,
                    DustID.Blood,
                    Main.rand.NextVector2Circular(3, 3),
                    150,
                    Color.DarkRed,
                    1.6f).noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item14, NPC.Center);

            Vector2 dash = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 35f;
            NPC.velocity = dash;
        }

        private void Teleport(Player player)
        {
            teleportPos = player.Center + Main.rand.NextVector2Circular(350, 350);

            for (int i = 0; i < 40; i++)
            {
                Dust.NewDustPerfect(NPC.Center,
                    DustID.Blood,
                    Main.rand.NextVector2Circular(5, 5),
                    150,
                    Color.Red,
                    2f).noGravity = true;
            }

            NPC.Center = teleportPos.Value;
        }

        // ================= ЭФФЕКТЫ =================

        private void SpawnPulseDust()
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustPerfect(NPC.Center,
                    DustID.Blood,
                    Main.rand.NextVector2Circular(2, 2),
                    150,
                    Color.Red,
                    1.4f).noGravity = true;
            }
        }

        // ================= ЛУТ =================

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.PlatinumCoin, 1, 3, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodEssence>(), 1, 4, 8));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CongealedBlood>(), 1, 100, 150));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 20, 25));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentVortex, 1, 20, 25));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentNebula, 1, 20, 25));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentStardust, 1, 20, 25));

        }

        public override void OnKill()
        {
            CompTechModSystem.downedExpiringCore = true;
        }
    }
}
