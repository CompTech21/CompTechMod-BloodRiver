using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace CompTechMod.Content.NPCs
{
    public class DestroyerAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int laserRainTimer = 0;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.TheDestroyer)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                float hpPercent = (float)npc.life / npc.lifeMax;

                // ===========================================================
                // 1. Фикс клубочка сегментов
                // ===========================================================
                bool tangled = false;
                foreach (NPC segment in Main.npc)
                {
                    if (segment.active && segment.realLife == npc.whoAmI && segment.type == NPCID.TheDestroyerBody)
                    {
                        if (Vector2.Distance(npc.Center, segment.Center) < 40f) // слишком близко
                        {
                            tangled = true;
                            break;
                        }
                    }
                }

                if (tangled)
                {
                    npc.defense = 9999; // почти неуязвим
                }
                else
                {
                    npc.defense = 30; // стандартная защита Destroyer
                }

                // ===========================================================
                // 2. До 50% HP → он таранит игрока
                // ===========================================================
                if (hpPercent > 0.5f)
                {
                    Vector2 dashDir = target.Center - npc.Center;
                    dashDir.Normalize();

                    // Сильное ускорение, как в Infernum
                    npc.velocity = Vector2.Lerp(npc.velocity, dashDir * 10f, 0.08f);
                }

                // ===========================================================
                // 3. После 50% HP → лазеры со всех сторон
                // ===========================================================
                if (hpPercent <= 0.5f)
                {
                    laserRainTimer++;
                    if (laserRainTimer >= 360) // каждые 6 сек
                    {
                        laserRainTimer = 0;

                        int spacing = 150;
                        int screenWidth = 1920;
                        int screenHeight = 1080;

                        // сверху вниз
                        for (int x = -screenWidth; x <= screenWidth; x += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + x, target.Center.Y - 1200);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(0, 8f),
                                ProjectileID.DeathLaser, 50, 3f, Main.myPlayer);
                        }

                        // снизу вверх
                        for (int x = -screenWidth; x <= screenWidth; x += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + x, target.Center.Y + 1200);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(0, -8f),
                                ProjectileID.DeathLaser, 50, 3f, Main.myPlayer);
                        }

                        // слева направо
                        for (int y = -screenHeight; y <= screenHeight; y += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X - 1600, target.Center.Y + y);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(8f, 0),
                                ProjectileID.DeathLaser, 50, 3f, Main.myPlayer);
                        }

                        // справа налево
                        for (int y = -screenHeight; y <= screenHeight; y += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + 1600, target.Center.Y + y);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(-8f, 0),
                                ProjectileID.DeathLaser, 50, 3f, Main.myPlayer);
                        }
                    }
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            // --- Взрывающиеся зонды ---
            if (npc.type == NPCID.Probe)
            {
                for (int i = 0; i < 40; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.FireworkFountain_Red,
                        Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                }

                for (int i = 0; i < 4; i++)
                {
                    Vector2 dir = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 90));
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 10f,
                        ProjectileID.DeathLaser, 30, 2f, Main.myPlayer);
                }
            }
        }
    }
}
