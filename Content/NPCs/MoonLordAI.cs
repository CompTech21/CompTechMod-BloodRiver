using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class MoonLordAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int eyeTimer = 0;
        private int sphereTimer = 0;
        private int labyrinthSpawned = 0;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.MoonLordCore)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                float hpPercent = (float)npc.life / npc.lifeMax;

                // ===========================================================
                // 1. Спиральные PhantasmalEye из рук и головы
                // ===========================================================
                eyeTimer++;
                if (eyeTimer >= 60) // каждую секунду
                {
                    eyeTimer = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 vel = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 30 + Main.GameUpdateCount % 360)) * 6f;
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vel,
                            ProjectileID.PhantasmalEye, 70, 3f, Main.myPlayer);
                    }
                }

                // ===========================================================
                // 2. Луч смерти всегда стреляет
                // ===========================================================
                if (Main.rand.NextBool(300)) // раз в ~5 сек
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY,
                        ProjectileID.PhantasmalDeathray, 120, 5f, Main.myPlayer);
                }

                // ===========================================================
                // 3. Круги из сфер вокруг игрока каждые 15 секунд
                // ===========================================================
                sphereTimer++;
                if (sphereTimer >= 900) // 15 сек
                {
                    sphereTimer = 0;
                    for (int r = 200; r <= 600; r += 200)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            Vector2 pos = target.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * (360 / 16))) * r;
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.Zero,
                                ProjectileID.PhantasmalSphere, 80, 4f, Main.myPlayer);
                        }
                    }
                }

                // ===========================================================
                // 4. Усиленный Moon Leech хил
                // ===========================================================
                if (npc.life < npc.lifeMax && npc.HasBuff(BuffID.MoonLeech))
                {
                    npc.life += 3000; 
                    if (npc.life > npc.lifeMax) npc.life = npc.lifeMax;
                }

                // ===========================================================
                // 5. <50% HP → лабиринт сфер
                // ===========================================================
                if (hpPercent <= 0.5f && labyrinthSpawned == 0)
                {
                    labyrinthSpawned = 1;
                    int spacing = 120;
                    int radius = 2000;

                    for (int x = -radius; x <= radius; x += spacing)
                    {
                        for (int y = -radius; y <= radius; y += spacing)
                        {
                            Vector2 pos = target.Center + new Vector2(x, y);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.Zero,
                                ProjectileID.PhantasmalSphere, 90, 4f, Main.myPlayer);
                        }
                    }
                }
            }
        }

        // ===========================================================
        // 6. Взрыв при смерти
        // ===========================================================
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.MoonLordCore)
            {
                for (int i = 0; i < 50; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.PurpleTorch, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f));
                }

                Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Vector2.Zero,
                    ProjectileID.PhantasmalDeathray, 1000, 10f, Main.myPlayer);
            }
        }
    }
}
