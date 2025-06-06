using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;
using CompTechMod.Content.Buffs;

namespace CompTechMod.Content.Items
{
    public class PhantomPactPlayer : ModPlayer
    {
        public bool hasPhantomPact;
        private bool wasEquippedLastTick;

        public override void ResetEffects()
        {
            hasPhantomPact = false;
        }

        public override void PreUpdate()
        {
            if (!hasPhantomPact && wasEquippedLastTick && Player.statLife > 0 && !Player.dead)
            {
                Player.KillMe(
                    PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{Player.name} broke the Ghost Pact.")),
                    Player.statLife + 9999,
                    0
                );
            }

            wasEquippedLastTick = hasPhantomPact;
        }

        public override void UpdateDead()
        {
            wasEquippedLastTick = false;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (hasPhantomPact && Player.statLife == 1)
            {
                bool hasDangerDebuff =
                    Player.HasBuff(BuffID.OnFire) ||
                    Player.HasBuff(BuffID.Burning) ||
                    Player.HasBuff(BuffID.CursedInferno) ||
                    Player.HasBuff(BuffID.Frostburn) ||
                    Player.HasBuff(BuffID.ShadowFlame) ||
                    Player.HasBuff(BuffID.Venom) ||
                    Player.HasBuff(ModContent.BuffType<EctoplasmDebuff>());

                if (!hasDangerDebuff)
                {
                    // Эффект уклонения
                    Player.NinjaDodge(); // проигрывает эффект, звук, мигание
                    Player.AddBuff(ModContent.BuffType<EctoplasmDebuff>(), 480); // 8 секунд
                    return true; // полностью избежать урон
                }
            }

            return false;
        }
    }
}