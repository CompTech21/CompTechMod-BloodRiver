using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CompTechMod.Content.Players
{
    public class GelatinShieldDashPlayer : ModPlayer
    {
        public const int DashDown = 0;
        public const int DashUp = 1;
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public const int DashCooldown = 60;
        public const int DashDuration = 20;
        public const float DashVelocity = 12f;

        public int DashDir = -1;

        public bool DashAccessoryEquipped;
        public int DashDelay = 0;
        public int DashTimer = 0;

        public override void ResetEffects()
        {
            DashAccessoryEquipped = false;

            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
                DashDir = DashDown;
            else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15)
                DashDir = DashUp;
            else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15 && Player.doubleTapCardinalTimer[DashLeft] == 0)
                DashDir = DashRight;
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15 && Player.doubleTapCardinalTimer[DashRight] == 0)
                DashDir = DashLeft;
            else
                DashDir = -1;
        }

        public override void PreUpdateMovement()
        {
            if (!CanUseDash())
                return;

            if (DashTimer > 0)
            {
                Player.eocDash = DashTimer;
                Player.armorEffectDrawOutlines = true;
                CreatePinkGelDust(Player.position);
                Player.immune = true;
                Player.immuneTime = 20;

                HitNPCsInDash(); // Наносим урон врагам

                DashTimer--;
            }

            if (DashDelay > 0)
                DashDelay--;

            if (DashDir == -1 || DashDelay > 0 || DashTimer > 0)
                return;

            Vector2 newVelocity = Player.velocity;

            switch (DashDir)
            {
                case DashUp when Player.velocity.Y > -DashVelocity:
                case DashDown when Player.velocity.Y < DashVelocity:
                    {
                        float dashDirection = DashDir == DashDown ? 1 : -1;
                        newVelocity.Y = dashDirection * DashVelocity;
                        break;
                    }
                case DashLeft when Player.velocity.X > -DashVelocity:
                case DashRight when Player.velocity.X < DashVelocity:
                    {
                        float dashDirection = DashDir == DashRight ? 1 : -1;
                        newVelocity.X = dashDirection * DashVelocity;
                        break;
                    }
                default:
                    return;
            }

            DashDelay = DashCooldown;
            DashTimer = DashDuration;
            Player.velocity = newVelocity;
        }

        private bool CanUseDash()
        {
            return DashAccessoryEquipped
                && Player.dashType == 0
                && !Player.setSolar
                && !Player.mount.Active;
        }

        private void CreatePinkGelDust(Vector2 position)
        {
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(position, Player.width, Player.height, DustID.PinkSlime);
                Main.dust[dust].scale = 1.4f;
                Main.dust[dust].velocity *= 1.8f;
                Main.dust[dust].noGravity = true;
            }
        }

        private void HitNPCsInDash()
        {
            Rectangle dashHitbox = Player.getRect();
            int damage = 60;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.active && !npc.friendly && !npc.dontTakeDamage && npc.Hitbox.Intersects(dashHitbox))
                {
                    Player.ApplyDamageToNPC(npc, damage, 0f, Player.direction, crit: false);
                    npc.netUpdate = true;
                }
            }
        }

    }
}
