using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class TwinsAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int laserTimer = 0;
        private int fireballTimer = 0;
        private int flameTimer = 0;
        private int dashTimer = 0;
        private int phase3AttackTimer = 0;

        private bool phase2Triggered = false;
        private bool phase3Triggered = false;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.Retinazer && npc.type != NPCID.Spazmatism)
                return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                return;

            float hpPercent = (float)npc.life / npc.lifeMax;

            // ====================================================
            // ФАЗА 1 — до 70% здоровья
            // ====================================================
            if (hpPercent > 0.7f)
            {
                if (npc.type == NPCID.Retinazer)
                {
                    // Лазеры быстрее и менее точные
                    laserTimer++;
                    int fireRate = (int)(45 * hpPercent + 10); // быстрее при меньшем хп
                    if (laserTimer >= fireRate)
                    {
                        laserTimer = 0;
                        Vector2 dir = Vector2.Normalize(target.Center - npc.Center);
                        dir = dir.RotatedByRandom(MathHelper.ToRadians(10)); // неточность
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * (12f + (1f - hpPercent) * 6f),
                            ProjectileID.EyeLaser, 20, 2f, Main.myPlayer);
                    }
                }
                else if (npc.type == NPCID.Spazmatism)
                {
                    // Быстрее стреляет огненными шарами (чуть медленнее летят, чем раньше)
                    fireballTimer++;
                    int fireRate = (int)(60 * hpPercent + 15); // быстрее при меньшем хп
                    if (fireballTimer >= fireRate)
                    {
                        fireballTimer = 0;
                        Vector2 dir = Vector2.Normalize(target.Center - npc.Center);
                        dir = dir.RotatedByRandom(MathHelper.ToRadians(6));
                        int proj = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * (8.5f + (1f - hpPercent) * 5.5f),
                            ProjectileID.CursedFlameHostile, 22, 2f, Main.myPlayer);
                        Main.projectile[proj].timeLeft = 120; // 2 секунды жизни
                    }
                }
            }

            // ====================================================
            // ФАЗА 2 — с 40% здоровья
            // ====================================================
            if (!phase2Triggered && hpPercent <= 0.4f)
                phase2Triggered = true;

            if (phase2Triggered && !phase3Triggered)
            {
                if (npc.type == NPCID.Spazmatism)
                {
                    // Чередование между атаками
                    dashTimer++;
                    if (dashTimer < 180) // 3 сек пламени
                    {
                        flameTimer++;
                        if (flameTimer >= 25) // пламя быстрее
                        {
                            flameTimer = 0;
                            Vector2 dir = Vector2.Normalize(target.Center - npc.Center);
                            int proj = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 8f,
                                ProjectileID.CursedFlameHostile, 25, 2f, Main.myPlayer);
                            Main.projectile[proj].timeLeft = 180; // 3 секунды
                        }
                    }
                    else if (dashTimer < 300) // 2 сек рывков
                    {
                        if (dashTimer % 80 == 0) // рывок
                        {
                            Vector2 dash = Vector2.Normalize(target.Center - npc.Center) * 11f;
                            npc.velocity = dash;
                        }
                    }
                    else
                    {
                        dashTimer = 0;
                    }
                }

                // Если один брат в фазе 2, а второй в фазе 1 → игрок наносит мало урона фазе 2
                int otherID = npc.type == NPCID.Retinazer ? NPCID.Spazmatism : NPCID.Retinazer;
                foreach (NPC other in Main.npc)
                {
                    if (other.active && other.type == otherID)
                    {
                        float otherHpPercent = (float)other.life / other.lifeMax;
                        if (otherHpPercent > 0.4f && hpPercent <= 0.4f)
                        {
                            npc.takenDamageMultiplier = 0f; // получает только 0% урона
                        }
                        else
                        {
                            npc.takenDamageMultiplier = 1f;
                        }
                    }
                }
            }

            // ====================================================
            // ФАЗА 3 — 25% здоровья или смерть брата
            // ====================================================
            if (!phase3Triggered && (hpPercent <= 0.25f || IsTwinDead(npc)))
                phase3Triggered = true;

            if (phase3Triggered)
            {
                phase3AttackTimer++;
                if (npc.type == NPCID.Spazmatism)
                {
                    // Проклятый дождь раз в 7 секунд (чуть быстрее падение)
                    if (phase3AttackTimer >= 420)
                    {
                        phase3AttackTimer = 0;
                        for (int i = -6; i <= 6; i++)
                        {
                            Vector2 pos = new Vector2(target.Center.X + i * 80, target.Center.Y - 600);
                            Vector2 vel = new Vector2(0, 14f); // чуть быстрее
                            int proj = Projectile.NewProjectile(npc.GetSource_FromAI(), pos, vel,
                                ProjectileID.CursedFlameHostile, 30, 2f, Main.myPlayer);
                            Main.projectile[proj].timeLeft = 180; // 3 секунды
                        }
                    }
                }
                else if (npc.type == NPCID.Retinazer)
                {
                    // Многократный залп лазеров раз в 5 секунд (чуть медленнее скорость)
                    if (phase3AttackTimer >= 300)
                    {
                        phase3AttackTimer = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            Vector2 dir = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 36));
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 8f, // раньше было 14f
                                ProjectileID.EyeLaser, 28, 2f, Main.myPlayer);
                        }
                    }
                }
            }
        }

        private bool IsTwinDead(NPC npc)
        {
            int otherID = npc.type == NPCID.Retinazer ? NPCID.Spazmatism : NPCID.Retinazer;
            foreach (NPC other in Main.npc)
            {
                if (other.active && other.type == otherID)
                    return false;
            }
            return true;
        }
    }
}
