using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class PlanteraAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int shootTimer = 0;
        private int volleyTimer = 0;
        private int chaosTimer = 0;
        private int thornTimer = 0;
        private bool usePoison = false; // переключатель для чередования

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.Plantera)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                float hpPercent = (float)npc.life / npc.lifeMax;

                // ===========================================================
                // 2. Первая фаза (100–75% HP) → быстрый спам по одному снаряду
                // ===========================================================
                if (hpPercent > 0.75f)
                {
                    shootTimer++;
                    if (shootTimer >= 15) // каждые 0.25 сек
                    {
                        shootTimer = 0;
                        int proj = usePoison ? ProjectileID.PoisonSeedPlantera : ProjectileID.SeedPlantera;
                        usePoison = !usePoison;

                        Vector2 vel = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY) * 10f;
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel,
                            proj, 30, 2f, Main.myPlayer);
                    }
                }

                // ===========================================================
                // 3. Фаза 75–50% HP → стреляет веером из 3 линий
                // ===========================================================
                else if (hpPercent > 0.5f)
                {
                    volleyTimer++;
                    if (volleyTimer >= 30) // каждые 0.5 сек
                    {
                        volleyTimer = 0;
                        int proj = usePoison ? ProjectileID.PoisonSeedPlantera : ProjectileID.SeedPlantera;
                        usePoison = !usePoison;

                        for (int i = -1; i <= 1; i++) // три угла
                        {
                            Vector2 dir = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY)
                                .RotatedBy(MathHelper.ToRadians(10 * i)); // веер
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 9f,
                                proj, 32, 2f, Main.myPlayer);
                        }
                    }
                }

                // ===========================================================
                // 4. Вторая фаза (≤50%) → хаотичная стрельба и ThornBall
                // ===========================================================
                else
                {

                    // --- Хаотичный мусор каждые 2 сек
                    chaosTimer++;
                    if (chaosTimer >= 120) // 2 сек
                    {
                        chaosTimer = 0;
                        int count = Main.rand.Next(8, 12);
                        for (int i = 0; i < count; i++)
                        {
                            int proj = Main.rand.NextBool() ? ProjectileID.SeedPlantera : ProjectileID.PoisonSeedPlantera;
                            Vector2 vel = (target.Center - npc.Center).SafeNormalize(Vector2.UnitY)
                                .RotatedByRandom(MathHelper.ToRadians(40)) * Main.rand.NextFloat(6f, 11f);

                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel,
                                proj, 36, 2f, Main.myPlayer);
                        }
                    }

                    // --- ThornBall каждые 2 сек
                    thornTimer++;
                    if (thornTimer >= 120)
                    {
                        thornTimer = 0;
                        Vector2 vel = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-9f, -6f));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel,
                            ProjectileID.ThornBall, 45, 3f, Main.myPlayer);
                    }
                }
            }
        }
    }
}
