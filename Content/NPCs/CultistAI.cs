using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CompTechMod.Content.NPCs
{
    public class CultistAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int attackTimer;
        private int teleportTimer;
        private int ritualTimer;

        private bool ritualActive;
        private bool dragonSummoned;

        private readonly List<int> ritualClones = new();

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.CultistBoss)
                return;

            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
                return;

            float hp = npc.life / (float)npc.lifeMax;

            attackTimer++;
            teleportTimer++;
            ritualTimer++;

            // =========================
            // РИТУАЛ
            // =========================
            if (ritualActive)
            {
                npc.velocity *= 0.92f;
                HandleRitualOrbit(npc, player);
                return;
            }

            // =========================
            // ПАРЕНИЕ (МЕДЛЕННО И ЧЕСТНО)
            // =========================
            Vector2 hoverPos = player.Center + new Vector2(0, -230);
            Vector2 move = hoverPos - npc.Center;

            npc.velocity = Vector2.Lerp(
                npc.velocity,
                move * 0.025f,
                0.08f
            );

            // =========================
            // PHASE 1 — РАЗМИНКА
            // =========================
            if (hp > 0.65f)
            {
                if (attackTimer >= 180)
                {
                    attackTimer = 0;
                    ShootAtPlayer(npc, player, 7f, 30);
                }
            }
            // =========================
            // PHASE 2 — РИТУАЛ
            // =========================
            else if (hp > 0.30f)
            {
                if (teleportTimer >= 320)
                {
                    teleportTimer = 0;
                    TeleportAroundPlayer(npc, player);
                }

                if (ritualTimer >= 520)
                {
                    ritualTimer = 0;
                    StartRitual(npc, player, 6);
                }

                if (attackTimer >= 220)
                {
                    attackTimer = 0;
                    ConeAttack(npc, player);
                }
            }
            // =========================
            // PHASE 3 — ФИНАЛ
            // =========================
            else
            {
                if (!dragonSummoned)
                {
                    dragonSummoned = true;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(
                            npc.GetSource_FromAI(),
                            (int)npc.Center.X,
                            (int)npc.Center.Y,
                            NPCID.CultistDragonHead
                        );
                    }
                }

                if (attackTimer >= 160)
                {
                    attackTimer = 0;
                    RingBurst(npc, 6, 7f);
                }
            }
        }

        // =========================
        // РИТУАЛ
        // =========================
        private void StartRitual(NPC npc, Player player, int count)
        {
            ritualActive = true;
            ritualTimer = 0;
            ritualClones.Clear();

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            for (int i = 0; i < count; i++)
            {
                int id = NPC.NewNPC(
                    npc.GetSource_FromAI(),
                    (int)npc.Center.X,
                    (int)npc.Center.Y,
                    NPCID.CultistBossClone
                );

                ritualClones.Add(id);
            }
        }

        private void HandleRitualOrbit(NPC npc, Player player)
        {
            ritualTimer++;

            float radius = 250f;
            float speed = 0.02f;
            float baseAngle = ritualTimer * speed;

            npc.Center = player.Center + baseAngle.ToRotationVector2() * radius;

            for (int i = 0; i < ritualClones.Count; i++)
            {
                int id = ritualClones[i];
                if (!Main.npc[id].active)
                    continue;

                float angle = baseAngle + MathHelper.TwoPi / ritualClones.Count * i;
                Main.npc[id].Center = player.Center + angle.ToRotationVector2() * radius;
            }

            if (ritualTimer >= 240)
            {
                ritualActive = false;
                KillRitualClones();
            }
        }

        private void KillRitualClones()
        {
            foreach (int id in ritualClones)
            {
                if (Main.npc[id].active)
                    Main.npc[id].StrikeInstantKill();
            }
            ritualClones.Clear();
        }

        // =========================
        // АТАКИ
        // =========================
        private void ShootAtPlayer(NPC npc, Player player, float speed, int dmg)
        {
            Vector2 dir = (player.Center - npc.Center).SafeNormalize(Vector2.UnitY);
            Projectile.NewProjectile(
                npc.GetSource_FromAI(),
                npc.Center,
                dir * speed,
                ProjectileID.CultistBossFireBall,
                dmg,
                2f
            );
        }

        private void ConeAttack(NPC npc, Player player)
        {
            Vector2 baseDir = (player.Center - npc.Center).SafeNormalize(Vector2.UnitY);

            for (int i = -1; i <= 1; i++)
            {
                Vector2 dir = baseDir.RotatedBy(MathHelper.ToRadians(i * 12));
                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    dir * 8f,
                    ProjectileID.CultistBossFireBall,
                    34,
                    2f
                );
            }
        }

        private void RingBurst(NPC npc, int count, float speed)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 dir = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / count * i);
                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    dir * speed,
                    ProjectileID.CultistBossFireBall,
                    38,
                    2f
                );
            }
        }

        private void TeleportAroundPlayer(NPC npc, Player player)
        {
            npc.Center = player.Center + Main.rand.NextVector2CircularEdge(280, 280);
        }
    }
}
