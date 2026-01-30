using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CompTechMod.Content.Projectiles
{
    public class SparkGunProj : ModProjectile
    {
        public bool IsStar;

        private int chaosTimer;
        private bool stopping;
        private bool exploded;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600; // длинная дистанция
            Projectile.light = 1f;
        }

        public override void AI()
        {
            chaosTimer++;

            float chaosStrength = IsStar ? 0.25f : 0.12f;

            // Хаотичный полёт
            Projectile.velocity += new Vector2(
                Main.rand.NextFloat(-chaosStrength, chaosStrength),
                Main.rand.NextFloat(-chaosStrength, chaosStrength)
            );

            int stopTime = IsStar ? Main.rand.Next(20, 50) : Main.rand.Next(40, 80);

            if (!stopping && chaosTimer > stopTime)
            {
                stopping = true;
            }

            if (stopping)
            {
                Projectile.velocity *= 0.95f;

                if (!exploded && Projectile.velocity.Length() < 0.3f)
                    Explode();
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            // Искры
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Electric
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return false;
        }

        [System.Obsolete]
        public override void Kill(int timeLeft)
        {
            if (!exploded)
                Explode();
        }

        private void Explode()
        {
            exploded = true;

            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            // Малый урон на столкновение
            int explosionDamage = (int)(Projectile.damage * 0.3f); // 30% урона
            Projectile.damage = explosionDamage; // переносим урон на сам Projectile

            // Визуальные эффекты
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Electric,
                    Main.rand.NextFloat(-2, 2),
                    Main.rand.NextFloat(-2, 2)
                );
                dust.noGravity = true;
                dust.scale = 1.2f;
            }

            Projectile.Kill();
        }
    }
}
