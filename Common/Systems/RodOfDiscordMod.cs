using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio; // <- обязательно

namespace CompTechMod.Common.Systems
{
    public class RodOfDiscordMod : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.RodofDiscord)
            {
                if (player.HasBuff(BuffID.ChaosState))
                    return false;
            }
            return base.CanUseItem(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (item.type == ItemID.RodofDiscord)
            {
                player.statLife += 0;
                player.AddBuff(BuffID.ChaosState, 11 * 60);
            }
            return base.UseItem(item, player);
        }
    }

    public class RodOfDiscordPlayer : ModPlayer
    {
        private bool hadChaosDebuff = false;

        public override void ResetEffects()
        {
            if (hadChaosDebuff && !Player.HasBuff(BuffID.ChaosState))
            {
                hadChaosDebuff = false;
                // Используем SoundEngine вместо Main.PlaySound
                SoundEngine.PlaySound(SoundID.Item28, Player.position);
            }

            if (Player.HasBuff(BuffID.ChaosState))
                hadChaosDebuff = true;
        }
    }
}
