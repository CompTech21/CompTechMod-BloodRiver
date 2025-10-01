using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class CultistAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int lightningTimer = 0;
        private int laserTimer = 0;
        private bool dragonSummoned = false;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.CultistBoss)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;

                float hpPercent = (float)npc.life / npc.lifeMax;

                // ===========================================================
                // 1. Призыв дракона сразу при спавне
                // ===========================================================
                if (!dragonSummoned)
                {
                    dragonSummoned = true;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.CultistDragonHead);
                    }
                }

                // ===========================================================
                // 2. До 50% HP → bullet-hell из молний
                // ===========================================================
                if (hpPercent <= 0.5f)
                {
                    lightningTimer++;
                    if (lightningTimer >= 200) // раз в 3 сек
                    {
                        lightningTimer = 0;

                        int spacing = 200; 
                        int screenWidth = 2000;
                        int screenHeight = 1200;

                        // Сверху вниз
                        for (int x = -screenWidth; x <= screenWidth; x += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + x, target.Center.Y - 1200);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(0, 5f),
                                ProjectileID.CultistBossLightningOrbArc, 50, 3f, Main.myPlayer);
                        }

                        // Снизу вверх
                        for (int x = -screenWidth; x <= screenWidth; x += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + x, target.Center.Y + 1200);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(0, -5f),
                                ProjectileID.CultistBossLightningOrbArc, 50, 3f, Main.myPlayer);
                        }

                        // Слева направо
                        for (int y = -screenHeight; y <= screenHeight; y += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X - 1600, target.Center.Y + y);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(5f, 0),
                                ProjectileID.CultistBossLightningOrbArc, 50, 3f, Main.myPlayer);
                        }

                        // Справа налево
                        for (int y = -screenHeight; y <= screenHeight; y += spacing)
                        {
                            Vector2 pos = new Vector2(target.Center.X + 1600, target.Center.Y + y);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), pos, new Vector2(-5f, 0),
                                ProjectileID.CultistBossLightningOrbArc, 50, 3f, Main.myPlayer);
                        }
                    }
                }

                // ===========================================================
                // 3. До 20% HP → StardustLaser в 8 направлений
                // ===========================================================
                if (hpPercent <= 0.2f)
                {
                    laserTimer++;
                    if (laserTimer >= 300) // каждые 5 сек
                    {
                        laserTimer = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 dir = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 45));
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 12f,
                                ProjectileID.StardustSoldierLaser, 60, 4f, Main.myPlayer);
                        }
                    }
                }
            }
        }
    }
}
