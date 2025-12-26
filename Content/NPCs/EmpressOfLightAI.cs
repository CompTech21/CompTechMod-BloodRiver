using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class EmpressOfLightAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int fanTimer;
        private int ringTimer;
        private int spiralTimer;
        private int cornerShotTimer;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.HallowBoss)
                return;

            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
                return;

            float hp = (float)npc.life / npc.lifeMax;
            bool daytime = Main.dayTime;

            // =========================
            // PHASE 1
            // =========================
            if (hp > 0.7f)
            {
                LightFan(npc, player, 150, 5, 7f, daytime);
                LightRing(npc, 360, 10, daytime);
            }
            // =========================
            // PHASE 2
            // =========================
            else if (hp > 0.35f)
            {
                LightFan(npc, player, 120, 6, 7.8f, daytime);
                SpiralPrisms(npc, 140, 8.5f, daytime);
                LightRing(npc, 300, 12, daytime);
                CornerShots(player, 420, daytime);
            }
            // =========================
            // PHASE 3
            // =========================
            else
            {
                LightFan(npc, player, 100, 7, 8.6f, daytime);
                SpiralPrisms(npc, 110, 9.5f, daytime);
                CornerShots(player, 300, daytime);
            }
        }

        // =====================================================
        // ВЕЕР СВЕТА
        // =====================================================
        private void LightFan(NPC npc, Player player, int delay, int count, float speed, bool daytime)
        {
            fanTimer++;
            if (fanTimer < delay)
                return;
            fanTimer = 0;

            int damage = daytime ? 9999 : 90;

            Vector2 baseDir = (player.Center - npc.Center).SafeNormalize(Vector2.UnitY);

            for (int i = -count; i <= count; i++)
            {
                Vector2 dir = baseDir.RotatedBy(MathHelper.ToRadians(i * 5));
                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    dir * speed,
                    ProjectileID.HallowBossRainbowStreak,
                    damage,
                    3f
                );
            }
        }

        // =====================================================
        // КОЛЬЦО СВЕТА
        // =====================================================
        private void LightRing(NPC npc, int delay, int count, bool daytime)
        {
            ringTimer++;
            if (ringTimer < delay)
                return;
            ringTimer = 0;

            int damage = daytime ? 9999 : 85;

            for (int i = 0; i < count; i++)
            {
                Vector2 dir = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / count * i);
                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    dir * 6.2f,
                    ProjectileID.HallowBossRainbowStreak,
                    damage,
                    2.5f
                );
            }
        }

        // =====================================================
        // СПИРАЛЬ
        // =====================================================
        private void SpiralPrisms(NPC npc, int delay, float speed, bool daytime)
        {
            spiralTimer++;
            if (spiralTimer < delay)
                return;
            spiralTimer = 0;

            int damage = daytime ? 9999 : 95;
            int count = 6;
            float baseAngle = Main.GameUpdateCount * 0.18f;

            for (int i = 0; i < count; i++)
            {
                float angle = baseAngle + MathHelper.TwoPi / count * i;
                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    angle.ToRotationVector2() * speed,
                    ProjectileID.HallowBossRainbowStreak,
                    damage,
                    3f
                );
            }
        }

        // =====================================================
        // АТАКА ИЗ УГЛОВ ЭКРАНА
        // =====================================================
        private void CornerShots(Player player, int delay, bool daytime)
        {
            cornerShotTimer++;
            if (cornerShotTimer < delay)
                return;
            cornerShotTimer = 0;

            int damage = daytime ? 9999 : 110;

            Vector2[] corners =
            {
                player.Center + new Vector2(-800, -450),
                player.Center + new Vector2(800, -450),
                player.Center + new Vector2(-800, 450),
                player.Center + new Vector2(800, 450),
            };

            foreach (Vector2 pos in corners)
            {
                Vector2 dir = (player.Center - pos).SafeNormalize(Vector2.UnitY);
                Projectile.NewProjectile(
                    player.GetSource_FromAI(),
                    pos,
                    dir * 9f,
                    ProjectileID.HallowBossRainbowStreak,
                    damage,
                    4f
                );
            }
        }
    }
}
