using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CompTechMod.Content.Projectiles
{
    public class OverheatExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 9;
        }

        public override void SetDefaults()
        {
            Projectile.width = 102;
            Projectile.height = 100;
            Projectile.timeLeft = 18;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
        }

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }

        public override void AI()
        {
            Projectile.Center = Main.player[Projectile.owner].Center;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 9)
                    Projectile.Kill();
            }
        }
    }
}
