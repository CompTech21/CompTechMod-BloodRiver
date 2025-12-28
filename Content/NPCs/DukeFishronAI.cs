using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class DukeFishronAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int sharkronRainTimer;
        private int missileTimer;
        private int bubbleTimer;
        private int postDashCooldown;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.DukeFishron)
                return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                return;

            // =================================
            // 1. ДОЖДЬ SHARKRON СВЕРХУ
            // =================================
            sharkronRainTimer++;
            if (sharkronRainTimer >= 200)
            {
                sharkronRainTimer = 0;

                for (int i = -1; i <= 1; i++)
                {
                    Vector2 spawnPos = target.Center + new Vector2(i * 180, -650);

                    NPC.NewNPC(
                        npc.GetSource_FromAI(),
                        (int)spawnPos.X,
                        (int)spawnPos.Y,
                        NPCID.Sharkron
                    );
                }
            }

            // =================================
            // 2. КД ПОСЛЕ РЫВКА
            // =================================
            if (npc.ai[0] == 2f) // ванильный рывок
            {
                postDashCooldown = 45;
            }

            if (postDashCooldown > 0)
                postDashCooldown--;

            // =================================
            // 3. FROST WAVE ПОСЛЕ РЫВКА (ЧЕСТНО)
            // =================================
            if (postDashCooldown == 1)
            {
                Vector2 dir = (target.Center - npc.Center).SafeNormalize(Vector2.UnitX);

                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    dir * 9f,
                    ProjectileID.FrostWave,
                    30,
                    2f,
                    Main.myPlayer
                );
            }

            // =================================
            // 4. DETONATING BUBBLE — КОНТРОЛЬ ЗОНЫ
            // =================================
            bubbleTimer++;
            if (bubbleTimer >= 240)
            {
                bubbleTimer = 0;

                Vector2 offset = new Vector2(
                    Main.rand.Next(-300, 301),
                    Main.rand.Next(-200, 201)
                );

                NPC.NewNPC(
                    npc.GetSource_FromAI(),
                    (int)(target.Center.X + offset.X),
                    (int)(target.Center.Y + offset.Y),
                    NPCID.DetonatingBubble
                );
            }

            // =================================
            // 5. SAUCER MISSILE — НАКАЗАНИЕ ЗА СТОЯНИЕ
            // =================================
            missileTimer++;
            if (missileTimer >= 160)
            {
                missileTimer = 0;

                Vector2 dir = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY);

                Projectile.NewProjectile(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    dir * 10f,
                    ProjectileID.SaucerMissile,
                    34,
                    2f,
                    Main.myPlayer
                );
            }

            // =================================
            // 6. ФАЗА ЯРОСТИ (<30% HP)
            // =================================
            if (npc.life < npc.lifeMax * 0.3f)
            {
                if (Main.rand.NextBool(90))
                {
                    Vector2 dir = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY);

                    Projectile.NewProjectile(
                        npc.GetSource_FromAI(),
                        npc.Center,
                        dir * 12f,
                        ProjectileID.FrostWave,
                        36,
                        2f,
                        Main.myPlayer
                    );
                }
            }
        }
    }
}
