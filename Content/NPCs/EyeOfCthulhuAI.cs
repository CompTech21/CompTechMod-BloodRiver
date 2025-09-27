using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using System;

namespace CompTechMod.Content.NPCs
{
    public class EyeOfCthulhuAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int phase = 1;
        private int dashCounter = 0;
        private int attackTimer = 0;

        private bool phase2Triggered = false;
        private bool lowHPTriggered30 = false;
        private bool lowHPTriggered15 = false;
        private bool lowHPTriggered5 = false;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.EyeofCthulhu) return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead) return;

            float hpPercent = (float)npc.life / npc.lifeMax;
            float distanceToPlayer = Vector2.Distance(npc.Center, target.Center);

            attackTimer++;

            // =============================
            // ПЕРЕХОД В ФАЗУ 2
            // =============================
            if (phase == 1 && hpPercent <= 0.6f && !phase2Triggered)
            {
                phase = 2;
                phase2Triggered = true;

                // Спавним 5–7 прислужников при переходе
                int servants = Main.rand.Next(5, 8);
                for (int i = 0; i < servants; i++)
                {
                    NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
                }
            }

        // =============================
        // ФАЗА 1 — лёгкая но напряжённая
        // =============================
        if (phase == 1)
        {
            if (distanceToPlayer > 320)
            {
                if (dashCounter < 2)
                {
                    DashAtPlayer(npc, target, 8f + dashCounter * 2f);
                    dashCounter++;
        
                    // Минимальный спавн прислужников (оставляем 0, чтобы не было слишком много)
                    int servants = 0;
                    for (int i = 0; i < servants; i++)
                        NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
                }
                else
                {
                    dashCounter = 0;
                    // Прямые выстрелы DemonSickle убраны
                }
            }
        
            // Bullet-Hell веером из 3 снарядов DemonSickle
            if (attackTimer % 120 == 0)
            {
                for (int i = -1; i <= 1; i++) // теперь 3 снаряда вместо 5
                {
                    Vector2 dir = Vector2.Normalize(target.Center - npc.Center).RotatedBy(MathHelper.ToRadians(i * 25));
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 10f, ProjectileID.DemonSickle, 20, 2f, Main.myPlayer);
                }
            }
        }

            // =============================
            // ФАЗА 2 — хаос и bullet hell
            // =============================
            if (phase == 2)
            {
                // Постоянный Bullet-Hell: DemonSickle + BloodShot
                if (attackTimer % 60 == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        float angle = MathHelper.ToRadians(i * 45); // 8 снарядов по кругу
                        Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 12f, ProjectileID.DemonSickle, 35, 2f, Main.myPlayer);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 10f, ProjectileID.BloodShot, 30, 2f, Main.myPlayer);
                    }
                }


                // События по HP
                if (hpPercent <= 0.3f && !lowHPTriggered30)
                {
                    lowHPTriggered30 = true;
                    PerformDashSequence(npc, target, 4, true);
                }

                if (hpPercent <= 0.15f && !lowHPTriggered15)
                {
                    lowHPTriggered15 = true;
                    PerformDashSequence(npc, target, 5, true);
                }

                if (hpPercent <= 0.05f && !lowHPTriggered5)
                {
                    lowHPTriggered5 = true;
                    PerformEnragedSequence(npc, target);
                }
            }
        }

        private void DashAtPlayer(NPC npc, Player target, float speed)
        {
            Vector2 direction = target.Center - npc.Center;
            direction.Normalize();
            npc.velocity = direction * speed;
        }

        private void DashStarPattern(NPC npc, Player target, float speed)
        {
            // Ры́вки в виде звезды Давида (6 направлений)
            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.ToRadians(i * 60);
                Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                npc.velocity = dir * speed;

                // Во время рывка создаём снаряды
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 14f, ProjectileID.DemonSickle, 35, 2f, Main.myPlayer);
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 12f, ProjectileID.BloodShot, 30, 2f, Main.myPlayer);
            }
        }

        private void PerformDashSequence(NPC npc, Player target, int quickDashes, bool horizontalCharge)
        {
            for (int i = 0; i < quickDashes; i++)
            {
                DashAtPlayer(npc, target, 18f);
            }

            if (horizontalCharge)
            {
                npc.velocity.X = (target.Center.X > npc.Center.X ? 1 : -1) * 20f;
                npc.velocity.Y = 0f;

                int servants = Main.rand.Next(5, 8);
                for (int i = 0; i < servants; i++)
                    NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
            }
        }

        private void PerformEnragedSequence(NPC npc, Player target)
        {
            // Очень быстрые рывки + Bullet-Hell
            for (int i = 0; i < 8; i++)
            {
                DashAtPlayer(npc, target, 22f);
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Normalize(target.Center - npc.Center) * 16f, ProjectileID.DemonSickle, 35, 2f, Main.myPlayer);
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Normalize(target.Center - npc.Center) * 14f, ProjectileID.BloodShot, 30, 2f, Main.myPlayer);
            }

            // Горизонтальный таран + спавн прислужников
            npc.velocity.X = (target.Center.X > npc.Center.X ? 1 : -1) * 26f;
            npc.velocity.Y = 0f;
            int servants = Main.rand.Next(7, 12);
            for (int i = 0; i < servants; i++)
                NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);

            // Дополнительные короткие рывки после тарана
            for (int i = 0; i < 4; i++)
            {
                DashAtPlayer(npc, target, 24f);
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Normalize(target.Center - npc.Center) * 18f, ProjectileID.DemonSickle, 35, 2f, Main.myPlayer);
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                int demonEyes = Main.rand.Next(6, 9);
                for (int i = 0; i < demonEyes; i++)
                    NPC.NewNPC(npc.GetSource_Death(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.DemonEye);
            }
        }
    }
}
