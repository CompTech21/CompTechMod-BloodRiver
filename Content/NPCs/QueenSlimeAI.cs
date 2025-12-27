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

        private int postTeleportCooldown;
        private int noContactDamageTimer;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.QueenSlimeBoss)
                return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                return;

            // ============================
            // Таймеры после телепорта
            // ============================
            if (postTeleportCooldown > 0)
                postTeleportCooldown--;

            if (noContactDamageTimer > 0)
                noContactDamageTimer--;

            // ============================
            // 1. Миньоны — умеренно
            // ============================
            minionTimer++;
            if (minionTimer >= 140)
            {
                minionTimer = 0;

                int count = Main.npc.Count(n =>
                    n.active &&
                    (n.type == NPCID.ShimmerSlime || n.type == NPCID.RainbowSlime)
                );

                if (count < 4)
                {
                    NPC.NewNPC(
                        npc.GetSource_FromAI(),
                        (int)npc.Center.X,
                        (int)npc.Center.Y,
                        Main.rand.NextBool() ? NPCID.ShimmerSlime : NPCID.RainbowSlime
                    );
                }
            }

            // ============================
            // 2. Кристальный дождь — честный
            // ============================
            crystalRainTimer++;
            if (crystalRainTimer >= 180)
            {
                crystalRainTimer = 0;

                for (int i = -1; i <= 1; i++)
                {
                    Vector2 spawnPos = target.Center + new Vector2(i * 180, -650);

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        spawnPos,
                        new Vector2(0, 8f),
                        ProjectileID.QueenSlimeMinionBlueSpike,
                        22,
                        2f,
                        Main.myPlayer
                    );
                }
            }

            // ============================
            // 3. Воздушный веер — с окнами
            // ============================
            projectileTimer++;
            if (projectileTimer >= 200 &&
                npc.velocity.Y != 0f &&
                postTeleportCooldown <= 0)
            {
                projectileTimer = 0;

                Vector2 dir = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY);

                int count = 5;
                float spread = MathHelper.ToRadians(24f);

                for (int i = 0; i < count; i++)
                {
                    Vector2 perturbed =
                        dir.RotatedBy(MathHelper.Lerp(-spread, spread, i / (float)(count - 1))) * 8f;

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        perturbed,
                        ProjectileID.HallowBossSplitShotCore,
                        25,
                        2f,
                        Main.myPlayer
                    );
                }
            }

            // ============================
            // 4. ЧЕСТНЫЙ телепорт
            // ============================
            teleportTimer++;
            if (teleportTimer >= 320)
            {
                teleportTimer = 0;

                // телеграф
                for (int i = 0; i < 30; i++)
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.HallowedPlants);

                Vector2 offset;
                do
                {
                    offset = Main.rand.NextVector2CircularEdge(700, 450);
                }
                while (offset.Length() < 500f); // ❗ минимум дистанции

                npc.Center = target.Center + offset;
                npc.velocity = Vector2.Zero;

                postTeleportCooldown = 60;
                noContactDamageTimer = 60;
            }

            // ============================
            // 5. Ярость — но без анфэйр
            // ============================
            if (npc.life < npc.lifeMax * 0.3f)
            {
                if (npc.velocity.Y > 0f && postTeleportCooldown <= 0)
                    npc.velocity.Y += 0.35f;

                if (Main.rand.NextBool(160) && postTeleportCooldown <= 0)
                {
                    Vector2 dir = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY) * 11f;

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        dir,
                        ProjectileID.HallowBossRainbowStreak,
                        28,
                        2f,
                        Main.myPlayer
                    );
                }
            }
        }

        // ============================
        // ❌ Убираем контактный урон
        // ============================
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (npc.type == NPCID.QueenSlimeBoss && noContactDamageTimer > 0)
                return false;

            return true;
        }
    }
}
