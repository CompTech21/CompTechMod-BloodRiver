using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class SkeletronAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        // таймеры для атак
        private int handShootTimer;
        private int headShootTimer;
        private int dashTimer;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.SkeletronHead)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;


                // --- Стрельба головой ---
                headShootTimer++;
                if (headShootTimer >= 400) // каждые 400 тиков
                {
                    headShootTimer = 0;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 dir = Vector2.Normalize(target.Center - npc.Center) * 10f;
                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center,
                            dir,
                            ProjectileID.Shadowflames,
                            20,
                            2f,
                            Main.myPlayer
                        );
                    }
                }

                // --- Рывки при <50% хп ---
                if (npc.life < npc.lifeMax / 2)
                {
                    dashTimer++;
                    if (dashTimer >= 120) // каждые 2 секунды
                    {
                        dashTimer = 0;
                        Vector2 dashDir = Vector2.Normalize(target.Center - npc.Center) * 14f;
                        npc.velocity = dashDir;
                    }
                }
            }

            // --- Руки Скелетрона ---
            if (npc.type == NPCID.SkeletronHand)
            {
                Player target = Main.player[npc.target];
                if (!target.active || target.dead) return;


                // Стрельба с рук Skull
                handShootTimer++;
                if (handShootTimer >= 107) // каждые 1,5 секунды
                {
                    handShootTimer = 0;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 dir = Vector2.Normalize(target.Center - npc.Center) * 10f;
                        Projectile.NewProjectile(
                            npc.GetSource_FromAI(),
                            npc.Center,
                            dir,
                            ProjectileID.Skull,
                            20,
                            1f,
                            Main.myPlayer
                        );
                    }
                }
            }
        }
    }
}
