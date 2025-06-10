using Terraria;
using Terraria.ModLoader;

namespace CompTechMod.Common.Players
{
    public class BloodyAscensionPlayer : ModPlayer
    {
        public bool bloodySet;
        public int regenTimer;

        public override void ResetEffects()
        {
            bloodySet = false;
        }

        public override void PostUpdate()
        {
            if (bloodySet && Player.statLife > Player.statLifeMax2 * 0.75f)
            {
                Player.GetDamage(DamageClass.Generic) += 0.10f;
                Player.GetCritChance(DamageClass.Generic) += 10f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TryHeal();
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            TryHeal();
        }

        private void TryHeal()
        {
            if (!bloodySet)
                return;

                if (Player.statLife < Player.statLifeMax2)
            {
                Player.statLife += 1;
                Player.HealEffect(1, true);
            }
        }

        }
    }
