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

namespace CompTechMod.Content.NPCs
{
    [AutoloadBossHead]
    public class ExpiringCore : ModNPC
    {
        private int phase = 1;
        private int attackTimer = 0;
        private int volleyCount = 0;

        private Vector2? pendingTeleport = null;
        private int teleportTimer = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 64;
            NPC.damage = Main.expertMode ? (Main.masterMode ? 400 : 300) : 200;
            NPC.defense = Main.expertMode ? (Main.masterMode ? 65 : 45) : 25;
            NPC.lifeMax = Main.expertMode ? (Main.masterMode ? 750000 : 400000) : 250000;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.aiStyle = -1;
            NPC.boss = true;
            Music = MusicID.Boss5;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest();
                if (!player.active || player.dead)
                {
                    NPC.velocity.Y -= 0.1f;
                    if (NPC.timeLeft > 10) NPC.timeLeft = 10;
                    return;
                }
            }

            // --- Проверка биома ---
            if (!(player.ZoneCrimson))
            {
                // игрок не в багрянце → босс неуязвим
                NPC.dontTakeDamage = true;

                // можно ещё подсветить эффектом
                Lighting.AddLight(NPC.Center, 1f, 0f, 0f); // красноватое свечение
            }
            else
            {
                // игрок снова в багрянце → босс уязвим
                NPC.dontTakeDamage = false;
            }

            // ФАЗЫ
            float hpPercent = (float)NPC.life / NPC.lifeMax;
            if (hpPercent <= 0.25f) phase = 4;
            else if (hpPercent <= 0.5f) phase = 3;
            else if (hpPercent <= 0.85f) phase = 2;
            else phase = 1;

            attackTimer++;

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

        private void PhaseOne(Player player)
        {
            MoveTowards(player.Center, 4f, 0.1f);

            if (attackTimer % 120 == 0)
            {
                ShootRing(player, 4);
            }
        }

        private void PhaseTwo(Player player)
        {
            MoveAbove(player.Center, 5f);

            if (attackTimer % 60 == 0)
            {
                ShootRing(player, 6);
                volleyCount++;
            }

            if (volleyCount >= 3)
            {
                DashAttack(player, 4);
                volleyCount = 0;
            }
        }

        private void PhaseThree(Player player)
        {
            MoveBelow(player.Center, 5f);

            if (attackTimer % 60 == 0)
            {
                ShootRing(player, 6);
                volleyCount++;
            }

            if (volleyCount >= 3)
            {
                DashAttack(player, 6);
                volleyCount = 0;
            }
        }

        private void PhaseFour(Player player)
        {
            MoveTowards(player.Center, 3f, 0.2f);

            if (attackTimer % 30 == 0)
            {
                ShootRing(player, 6);
            }

            // Запускаем телепорт каждые 5 секунд
            if (attackTimer % 300 == 0 && pendingTeleport == null)
            {
                pendingTeleport = player.Center + new Vector2(Main.rand.Next(-400, 401), Main.rand.Next(-400, 401));
                teleportTimer = 240; // 4 секунды предупреждения

                // создаём пыль в точке будущего телепорта
                for (int i = 0; i < 40; i++)
                {
                    Vector2 dustPos = pendingTeleport.Value + Main.rand.NextVector2Circular(50, 50);
                    Dust.NewDustPerfect(dustPos, DustID.Blood, Main.rand.NextVector2Circular(2, 2), 150, Color.Red, 1.5f).noGravity = true;
                }
            }

            // отсчёт таймера для телепорта
            if (pendingTeleport != null)
            {
                teleportTimer--;
                if (teleportTimer <= 0)
                {
                    NPC.Center = pendingTeleport.Value;
                    pendingTeleport = null;
                }
            }
        }

        private void MoveTowards(Vector2 target, float speed, float inertia)
        {
            Vector2 move = target - NPC.Center;
            float magnitude = move.Length();
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            NPC.velocity = (NPC.velocity * (1f - inertia)) + (move * inertia);
        }

        private void MoveAbove(Vector2 target, float speed)
        {
            Vector2 above = target + new Vector2(0, -200);
            MoveTowards(above, speed, 0.1f);
        }

        private void MoveBelow(Vector2 target, float speed)
        {
            Vector2 below = target + new Vector2(0, 200);
            MoveTowards(below, speed, 0.1f);
        }

        private void ShootRing(Player player, int numProjectiles)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                Vector2 velocity = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numProjectiles * i)) * 8f;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<BloodBlast>(), NPC.damage / 4, 2f);
            }
        }

        private void DashAttack(Player player, int numProjectiles)
        {
            Vector2 dashDir = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 40f;
            NPC.velocity = dashDir;
            ShootRing(player, numProjectiles);
            ShootRing(player, numProjectiles);
        }


        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.PlatinumCoin, 1, 2, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.GreaterHealingPotion, 1, 5, 15));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 20, 25));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentVortex, 1, 20, 25));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentNebula, 1, 20, 25));
            npcLoot.Add(ItemDropRule.Common(ItemID.FragmentStardust, 1, 20, 25));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CongealedBlood>(), 1, 100, 150));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodEssence>(), 1, 4, 8));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry entry)
        {
            entry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson,
            });
        }

        public override void OnKill()
        {
            CompTechModSystem.downedExpiringCore = true;
        }
    }
}
