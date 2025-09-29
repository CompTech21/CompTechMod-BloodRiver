using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class PrimeAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int laserTimer = 0;
        private int skullTimer = 0;
        private int dashTimer = 0;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.SkeletronPrime)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                float hpPercent = (float)npc.life / npc.lifeMax;

                // ===========================================================
                // 1. DeathLaser в 12 направлений каждые 7 секунд
                // ===========================================================
                laserTimer++;
                if (laserTimer >= 420) // 7 секунд
                {
                    laserTimer = 0;

                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 dir = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 30));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 6f, // медленнее ваниллы
                            ProjectileID.DeathLaser, 40, 3f, Main.myPlayer);
                    }
                }

                // ===========================================================
                // 2. До 10% HP → рывки при вращении
                // ===========================================================
                if (hpPercent <= 0.1f && npc.ai[1] == 1f) // ai[1] == 1 → фаза вращения
                {
                    dashTimer++;
                    if (dashTimer >= 90) // рывок раз в 1.5 сек
                    {
                        dashTimer = 0;
                        Vector2 dashDir = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY);
                        npc.velocity = dashDir * 20f; // быстрый рывок
                    }
                }

                // ===========================================================
                // 3. До 30% HP → спамит черепами (Skull)
                // ===========================================================
                if (hpPercent <= 0.3f)
                {
                    skullTimer++;
                    if (skullTimer >= 25) // каждые ~0.4 сек
                    {
                        skullTimer = 0;

                        Vector2 vel = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY) * 8f;
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel,
                            ProjectileID.Skull, 35, 3f, Main.myPlayer);
                    }
                }

                // ===========================================================
                // 4. 5% HP → меняет время на 4:40 и снижает скорость погони
                // ===========================================================
                if (hpPercent <= 0.05f)
                {
                    Main.dayTime = false;
                    Main.time = 4 * 3600 + 40 * 60; // 4:40

                    // Чтобы не гонял игрока слишком быстро днём
                    if (npc.velocity.Length() > 10f)
                        npc.velocity *= 0.95f; // плавное замедление
                }
            }
        }
    }
}
