using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
        private int despawnTimer;

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

            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/ExpiringCoreOst");
            SceneEffectPriority = SceneEffectPriority.BossHigh;
        }

        public override void AI()
        {
            if (!AnyAlivePlayer())
            {
                DespawnBehavior();
                return;
            }

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest();
                return;
            }

            despawnTimer = 0;

            NPC.dontTakeDamage = !player.ZoneCrimson;

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
                case 1: PhaseOne(player); break;
                case 2: PhaseTwo(player); break;
                case 3: PhaseThree(player); break;
                case 4: PhaseFour(player); break;
            }
        }

        // ================= ДЕСПАВН =================

        private bool AnyAlivePlayer()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (p.active && !p.dead)
                    return true;
            }
            return false;
        }

        private void DespawnBehavior()
        {
            despawnTimer++;

            NPC.velocity = Vector2.Lerp(
                NPC.velocity,
                new Vector2(0f, -20f),
                0.05f
            );

            NPC.rotation += 0.1f;
            NPC.dontTakeDamage = true;

            if (despawnTimer > 180)
                NPC.active = false;
        }

        // ================= ФАЗЫ =================

        private void PhaseOne(Player player)
        {
            Hover(player, 240);
            if (attackTimer % 90 == 0)
                RadialShot(6, 7f);
        }

        private void PhaseTwo(Player player)
        {
            Hover(player, 180);
            if (attackTimer % 120 == 0)
                SpiralShot(16, 8f);
            if (attackTimer % 240 == 0)
                Dash(player);
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
                Dash(player);
        }

        // ================= ДВИЖЕНИЕ =================

        private void Hover(Player player, float height)
        {
            Vector2 target = player.Center + new Vector2(0, -height);
            NPC.velocity = Vector2.Lerp(NPC.velocity, (target - NPC.Center) * 0.05f, 0.08f);
        }

        private void Orbit(Player player, float radius, float speed)
        {
            Vector2 pos = player.Center +
                new Vector2((float)Math.Cos(rotationAngle), (float)Math.Sin(rotationAngle)) * radius;

            NPC.velocity = (pos - NPC.Center) * speed;
        }

        // ================= АТАКИ =================

        private void RadialShot(int count, float speed)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 vel = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / count * i) * speed;
                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    vel,
                    ModContent.ProjectileType<BloodBlast>(),
                    NPC.damage / 4,
                    2f
                );
            }
        }

        private void SpiralShot(int count, float speed)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 vel = Vector2.UnitX
                    .RotatedBy(rotationAngle + MathHelper.TwoPi / count * i) * speed;

                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    vel,
                    ModContent.ProjectileType<BloodBlast>(),
                    NPC.damage / 4,
                    2f
                );
            }
        }

        private void Dash(Player player)
        {
            NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 35f;
            SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
        }

        private void Teleport(Player player)
        {
            NPC.Center = player.Center + Main.rand.NextVector2Circular(350, 350);
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
