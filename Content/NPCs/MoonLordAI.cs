using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class MoonLordAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int eyeTimer;
        private int deathrayTimer;
        private int sphereWaveTimer;

        private bool finalPhaseStarted;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.MoonLordCore)
                return;

            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
                return;

            float hp = (float)npc.life / npc.lifeMax;

            // =====================================
            // PHASE 1 — усиленная ванилла (>70%)
            // =====================================
            if (hp > 0.7f)
            {
                SpiralBurst(npc, 6, 6f, 90);
            }

            // =====================================
            // PHASE 2 — давление (70%–35%)
            // =====================================
            else if (hp > 0.35f)
            {
                SpiralBurst(npc, 8, 6.5f, 80);
                TimedDeathray(npc, player, 420);
                SphereRing(player, 420);
            }

            // =====================================
            // PHASE 3 — финал (<35%)
            // =====================================
            else
            {
                SpiralBurst(npc, 10, 7.5f, 65);
                TimedDeathray(npc, player, 300);

                if (!finalPhaseStarted)
                {
                    finalPhaseStarted = true;
                    sphereWaveTimer = 0;
                }

                SphereWalls(player);
            }
        }

        // =====================================================
        // СПИРАЛЬНЫЙ BURST (НЕ ПРИВЯЗАН К ДВИЖЕНИЮ)
        // =====================================================
        private void SpiralBurst(NPC npc, int count, float speed, int delay)
        {
            eyeTimer++;
            if (eyeTimer < delay)
                return;

            eyeTimer = 0;

            float offset = Main.GameUpdateCount * 0.04f;

            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.TwoPi / count * i + offset;
                Vector2 velocity = angle.ToRotationVector2() * speed;

                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    velocity,
                    ProjectileID.PhantasmalEye,
                    65,
                    3f
                );
            }
        }

        // =====================================================
        // DEATHRAY — РЕДКИЙ И ЧИТАЕМЫЙ
        // =====================================================
        private void TimedDeathray(NPC npc, Player player, int cooldown)
        {
            deathrayTimer++;
            if (deathrayTimer < cooldown)
                return;

            deathrayTimer = 0;

            Vector2 dir = (player.Center - npc.Center).SafeNormalize(Vector2.UnitY);

            Projectile.NewProjectile(
                npc.GetSource_FromAI(),
                npc.Center,
                dir,
                ProjectileID.PhantasmalDeathray,
                130,
                6f
            );
        }

        // =====================================================
        // КОЛЬЦО СФЕР ВОКРУГ ИГРОКА
        // =====================================================
        private void SphereRing(Player player, int cooldown)
        {
            sphereWaveTimer++;
            if (sphereWaveTimer < cooldown)
                return;

            sphereWaveTimer = 0;

            int count = 10;
            float radius = 360f;

            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.TwoPi / count * i;
                Vector2 pos = player.Center + angle.ToRotationVector2() * radius;

                Projectile.NewProjectile(
                    player.GetSource_FromAI(),
                    pos,
                    Vector2.Zero,
                    ProjectileID.PhantasmalSphere,
                    75,
                    3f
                );
            }
        }

        // =====================================================
        // ФИНАЛ — СТЕНЫ, А НЕ СПАМ
        // =====================================================
        private void SphereWalls(Player player)
        {
            sphereWaveTimer++;
            if (sphereWaveTimer < 120)
                return;

            sphereWaveTimer = 0;

            int spacing = 180;
            int length = 6;

            Vector2 dir = Main.rand.NextBool()
                ? Vector2.UnitX
                : Vector2.UnitY;

            for (int i = -length; i <= length; i++)
            {
                Vector2 pos = player.Center + dir * i * spacing;

                Projectile.NewProjectile(
                    player.GetSource_FromAI(),
                    pos,
                    Vector2.Zero,
                    ProjectileID.PhantasmalSphere,
                    85,
                    3f
                );
            }
        }

        // =====================================================
        // СМЕРТЬ — ЧИСТО ВИЗУАЛ
        // =====================================================
        public override void OnKill(NPC npc)
        {
            if (npc.type != NPCID.MoonLordCore)
                return;

            for (int i = 0; i < 40; i++)
            {
                Dust.NewDust(
                    npc.position,
                    npc.width,
                    npc.height,
                    DustID.PurpleTorch,
                    Main.rand.NextFloat(-4f, 4f),
                    Main.rand.NextFloat(-4f, 4f)
                );
            }
        }
    }
}
