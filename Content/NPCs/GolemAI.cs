using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class GolemAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int jumpTimer = 0;
        private int eyeBeamTimer = 0;
        private int headEyeBeamTimer = 0;
        private int fireballTimer = 0;
        private int fireballBurst = 0;

        public override void AI(NPC npc)
        {
            // --- Тело Голема ---
            if (npc.type == NPCID.Golem)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                float hpPercent = (float)npc.life / npc.lifeMax;

                // ===========================================================
                // 1. Первая фаза → короткие частые прыжки
                // ===========================================================
                if (hpPercent > 0.5f)
                {

                }

                // ===========================================================
                // 2. Вторая фаза (≤50% HP) → EyeBeam bullet-hell
                // ===========================================================
                else
                {
                    eyeBeamTimer++;
                    if (eyeBeamTimer >= 420) // каждые 7 сек
                    {
                        eyeBeamTimer = 0;

                        int spacing = 160; // расстояние между лазерами
                        int screenWidth = 1920;
                        int screenHeight = 1080;

                        // сверху вниз
                        for (int x = -screenWidth; x <= screenWidth; x += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + x, target.Center.Y - 1200);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(0, 2f),
                                ProjectileID.EyeBeam, 40, 3f, Main.myPlayer);
                        }

                        // снизу вверх
                        for (int x = -screenWidth; x <= screenWidth; x += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + x, target.Center.Y + 1200);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(0, -2f),
                                ProjectileID.EyeBeam, 40, 3f, Main.myPlayer);
                        }

                        // слева направо
                        for (int y = -screenHeight; y <= screenHeight; y += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X - 1600, target.Center.Y + y);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(2f, 0),
                                ProjectileID.EyeBeam, 40, 3f, Main.myPlayer);
                        }

                        // справа налево
                        for (int y = -screenHeight; y <= screenHeight; y += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + 1600, target.Center.Y + y);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(-2f, 0),
                                ProjectileID.EyeBeam, 40, 3f, Main.myPlayer);
                        }
                    }
                }
            }

            // --- Голова Голема ---
            if (npc.type == NPCID.GolemHeadFree) // отсоединённая голова
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                // ===========================================================
                // 3. Голова стреляет EyeBeam в 6 направлений
                // ===========================================================
                headEyeBeamTimer++;
                if (headEyeBeamTimer >= 180) // каждые 3 сек
                {
                    headEyeBeamTimer = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 dir = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 60));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 3f,
                            ProjectileID.EyeBeam, 35, 2f, Main.myPlayer);
                    }
                }

                // ===========================================================
                // 4. Голова каждые 5 сек выпускает 3 Fireball
                // ===========================================================
                fireballTimer++;
                if (fireballTimer >= 300) // каждые 5 сек
                {
                    fireballTimer = 0;
                    fireballBurst = 3; // 3 выстрела подряд
                }

                if (fireballBurst > 0 && fireballTimer % 20 == 0) // пауза между шарами
                {
                    fireballBurst--;
                    Vector2 vel = (target.Center - npc.Center)
                        .SafeNormalize(Vector2.UnitY)
                        .RotatedByRandom(MathHelper.ToRadians(25)) * Main.rand.NextFloat(5f, 9f);

                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel,
                        ProjectileID.Fireball, 45, 3f, Main.myPlayer);
                }
            }
        }
    }
}
