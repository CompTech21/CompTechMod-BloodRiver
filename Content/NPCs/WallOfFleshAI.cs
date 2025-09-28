using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class WallOfFleshAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int demonSickleTimer = 0;
        private int pinkLaserTimer = 0;
        private bool phase66Triggered = false;
        private bool phase30Triggered = false;
        private bool phase5Triggered = false;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.WallofFlesh) return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead) return;

            float hpPercent = (float)npc.life / npc.lifeMax;

            // ==============================
            // 1. Увеличиваем базовую скорость на 65%
            // ==============================
            npc.velocity *= 1.65f;

            // ==============================
            // 2. Фаза 66% HP — стреляет DemonSickle из рта
            // ==============================
            if (!phase66Triggered && hpPercent <= 0.66f)
                phase66Triggered = true;

            if (phase66Triggered)
            {
                demonSickleTimer++;
                if (demonSickleTimer >= 320) // каждые 7 секунд
                {
                    demonSickleTimer = 0;
                    ShootDemonSickle(npc, target);
                }
            }

            // ==============================
            // 3. Фаза 30% HP — стреляет PinkLaser из глаз
            // ==============================
            if (!phase30Triggered && hpPercent <= 0.3f)
                phase30Triggered = true;

            if (phase30Triggered)
            {
                pinkLaserTimer++;
                if (pinkLaserTimer >= 30) // каждые 0.5 сек
                {
                    pinkLaserTimer = 0;
                    ShootPinkLaser(npc, target);
                }
            }

            // ==============================
            // 4. Фаза 5% HP — супер ускорение и непрерывные атаки
            // ==============================
            if (!phase5Triggered && hpPercent <= 0.05f)
                phase5Triggered = true;

            if (phase5Triggered)
            {
                npc.velocity *= 1.25f;

                // DemonSickle каждые 2.5 сек
                demonSickleTimer++;
                if (demonSickleTimer >= 150)
                {
                    demonSickleTimer = 0;
                    ShootDemonSickle(npc, target);
                }

                // PinkLaser каждые 0.5 сек
                pinkLaserTimer++;
                if (pinkLaserTimer >= 30)
                {
                    pinkLaserTimer = 0;
                    ShootPinkLaser(npc, target);
                }
            }

            // ==============================
            // 5. Догоняет игрока, если он ушёл >200 блоков
            // ==============================
            float distance = Vector2.Distance(npc.Center, target.Center);
            if (distance >= 200 * 16) // 200 блоков
            {
                Vector2 dir = Vector2.Normalize(target.Center - npc.Center);
                npc.velocity += dir * 1.5f; // ускорение 150%
            }
            else if (distance <= 30 * 16) // сброс скорости, если близко
            {
                npc.velocity /= 1.5f;
            }
        }

        // ==============================
        // Стреляет одной DemonSickle в линию
        // ==============================
        private void ShootDemonSickle(NPC npc, Player target)
        {
            Vector2 dir = Vector2.Normalize(target.Center - npc.Center);
            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 10f, ProjectileID.DemonSickle, 30, 3f, Main.myPlayer);
        }

        // ==============================
        // Стреляет одним PinkLaser в линию
        // ==============================
        private void ShootPinkLaser(NPC npc, Player target)
        {
            Vector2 dir = Vector2.Normalize(target.Center - npc.Center);
            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 12f, ProjectileID.PinkLaser, 25, 2f, Main.myPlayer);
        }
    }
}
