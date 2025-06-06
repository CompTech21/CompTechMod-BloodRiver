using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CompTechMod.Common.Systems
{
    public class StrawberryModPlayer : ModPlayer
    {
        public bool hasEatenStrawberry;

        public override void UpdateEquips()
        {
            if (hasEatenStrawberry)
            {
                Player.statLifeMax2 += 25;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["hasEatenStrawberry"] = hasEatenStrawberry;
        }

        public override void LoadData(TagCompound tag)
        {
            hasEatenStrawberry = tag.GetBool("hasEatenStrawberry");
        }

        public static bool HasEaten(Player player) => player.GetModPlayer<StrawberryModPlayer>().hasEatenStrawberry;
        public static void SetEaten(Player player, bool value) => player.GetModPlayer<StrawberryModPlayer>().hasEatenStrawberry = value;
    }
}