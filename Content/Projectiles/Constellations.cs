using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CompTechMod.Content.Projectiles
{
    public class Constellations : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.light = 0.5f;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f * Projectile.direction;

            // Простой пыль
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].scale = 1.2f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // Взрыв пыли при смерти
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst);
                Main.dust[dust].velocity *= 1.5f;
                Main.dust[dust].scale = 1.3f;
                Main.dust[dust].noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}