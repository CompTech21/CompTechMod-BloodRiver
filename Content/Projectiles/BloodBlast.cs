using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Buffs;

namespace CompTechMod.Content.Projectiles
{
    public class BloodBlast : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 18;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            // ===== ЯРКИЙ КРАСНЫЙ СВЕТ =====
            Lighting.AddLight(
                Projectile.Center,
                1.4f, // R
                0.1f, // G
                0.1f  // B
            );

            // ===== СВЕТОВЫЕ ЧАСТИЦЫ =====
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Blood,
                    Projectile.velocity * 0.1f,
                    150,
                    Color.Red,
                    1.5f
                );
                dust.noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage > 0)
            {
                target.AddBuff(
                    ModContent.BuffType<BleedingBloodDebuff>(),
                    120
                );
            }
        }
    }
}
