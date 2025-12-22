using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Buffs;

namespace CompTechMod.Content.Projectiles
{
    public class BloodBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

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
            AIType = ProjectileID.Bullet;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage > 0)
            {
                // накладывает твой дебафф
                target.AddBuff(ModContent.BuffType<BleedingBloodDebuff>(), 120); // 2 сек
            }
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
            DustID.Blood, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 150, Color.Red, 1.2f);
        }
    }
}
