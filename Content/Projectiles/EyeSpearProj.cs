using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Projectiles
{
    public class EyeSpearProj : ModProjectile
    {
        protected virtual float HoldoutRangeMin => 24f;
        protected virtual float HoldoutRangeMax => 96f;

        public override void SetDefaults() {
            Projectile.CloneDefaults(ProjectileID.Spear);
        }

        public override bool PreAI() {
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;

            player.heldProj = Projectile.whoAmI;

            if (Projectile.timeLeft > duration)
                Projectile.timeLeft = duration;

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            float halfDuration = duration * 0.5f;
            float progress = Projectile.timeLeft < halfDuration
                ? Projectile.timeLeft / halfDuration
                : (duration - Projectile.timeLeft) / halfDuration;

            Projectile.Center = player.MountedCenter +
                Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

            Projectile.rotation = Projectile.spriteDirection == -1
                ? Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f)
                : Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

            // Эффект багряного факела — кровь
            if (!Main.dedServ) {
                if (Main.rand.NextBool(2)) {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CrimsonTorch);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale = 1.3f;
                    Main.dust[dust].velocity *= 0.3f;
                }
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Наложение дебаффа "Кроваво разделан" (ID 344) на 5 секунд
            target.AddBuff(30, 300);
        }
    }
}
