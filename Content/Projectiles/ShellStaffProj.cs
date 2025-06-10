using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Projectiles
{
    public class ShellStaffProj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false; // Проходит сквозь блоки
            Projectile.light = 0.3f; // Яркость света
            Projectile.aiStyle = -1; // Отключаем стандартное поведение
        }

        public override void AI()
        {
            // Вращаем снаряд для визуала
            Projectile.rotation += 0.1f;

            // Можно добавить небольшой свет по цвету ракушки
            Lighting.AddLight(Projectile.Center, 0.2f, 0.3f, 0.1f); // зеленоватый свет
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Можно добавить эффект при попадании, если хочешь
            for (int i = 0; i < 5; i++)
                Dust.NewDust(target.position, target.width, target.height, DustID.Sand);
        }
    }
}
