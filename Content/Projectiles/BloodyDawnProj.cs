using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CompTechMod.Content.Projectiles
{
	public class BloodyDawnProj : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f; // Бесконечное время действия
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 560f; // 35 блоков * 16 пикселей
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 70f;
		}

		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = ProjAIStyleID.Yoyo;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.penetrate = -1;
		}

		public override void PostAI() {
			if (Main.rand.NextBool(4)) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(70, 300, false);
            target.AddBuff(23, 300, false);
            target.AddBuff(69, 300, false);
        }
	}
}
