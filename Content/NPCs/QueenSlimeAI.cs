using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace CompTechMod.Content.NPCs
{
    public class QueenSlimeAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int crystalRainTimer;
        private int teleportTimer;
        private int projectileTimer;
        private int minionTimer;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.QueenSlimeBoss)
                return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                return;

            // ============================
            // 1. Спавн кристальных слизней
            // ============================
            minionTimer++;
            if (minionTimer >= 90)
            {
                minionTimer = 0;

                int count = Main.npc.Count(n =>
                    n.active &&
                    (n.type == NPCID.ShimmerSlime || n.type == NPCID.RainbowSlime)
                );

                if (count < 6)
                {
                    int type = Main.rand.NextBool()
                        ? NPCID.ShimmerSlime
                        : NPCID.RainbowSlime;

                    NPC.NewNPC(
                        npc.GetSource_FromAI(),
                        (int)npc.Center.X,
                        (int)npc.Center.Y,
                        type
                    );
                }
            }

            // ============================
            // 2. ДОЖДЬ ИЗ ШАРОВ СВЕРХУ
            // ============================
            crystalRainTimer++;
            if (crystalRainTimer >= 120)
            {
                crystalRainTimer = 0;

                for (int i = -2; i <= 2; i++)
                {
                    Vector2 spawnPos = target.Center + new Vector2(i * 120, -700);
                    Vector2 velocity = new Vector2(0, 12f);

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        spawnPos,
                        velocity,
                        ProjectileID.QueenSlimeMinionBlueSpike,
                        30,
                        3f,
                        Main.myPlayer
                    );
                }
            }

            // ============================
            // 3. АТАКА В ВОЗДУХЕ (ВЕЕР)
            // ============================
            projectileTimer++;
            if (projectileTimer >= 150 && npc.velocity.Y != 0f)
            {
                projectileTimer = 0;

                Vector2 direction = target.Center - npc.Center;
                direction.Normalize();

                int count = 7;
                float spread = MathHelper.ToRadians(35f);

                for (int i = 0; i < count; i++)
                {
                    Vector2 perturbed =
                        direction.RotatedBy(MathHelper.Lerp(-spread, spread, i / (float)(count - 1))) * 9f;

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        perturbed,
                        ProjectileID.HallowBossSplitShotCore,
                        28,
                        2f,
                        Main.myPlayer
                    );
                }
            }

            // ============================
            // 4. БЫСТРЫЕ ТЕЛЕПОРТЫ
            // ============================
            teleportTimer++;
            if (teleportTimer >= 240)
            {
                teleportTimer = 0;

                Vector2 offset = new Vector2(
                    Main.rand.Next(-400, 401),
                    Main.rand.Next(-300, -100)
                );

                npc.Center = target.Center + offset;
                npc.velocity = Vector2.Zero;

                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.HallowedPlants);
                }
            }

            // ============================
            // 5. ФАЗА ЯРОСТИ (<30% HP)
            // ============================
            if (npc.life < npc.lifeMax * 0.3f)
            {
                npc.knockBackResist = 0f;

                if (npc.velocity.Y > 0f)
                    npc.velocity.Y += 0.6f;

                if (Main.rand.NextBool(80))
                {
                    Vector2 dir = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY) * 14f;

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        dir,
                        ProjectileID.HallowBossRainbowStreak,
                        35,
                        3f,
                        Main.myPlayer
                    );
                }
            }
        }
    }
}
