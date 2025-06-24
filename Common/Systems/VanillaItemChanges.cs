using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class VanillaItemChanges : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.CopperPickaxe)
            {
                item.useTime = 11;
                item.useAnimation = 19;
                item.pick = 45;
            }

            if (item.type == ItemID.CopperAxe)
            {
                item.useTime = 18;
                item.useAnimation = 26;
                item.axe = 9;
            }

            if (item.type == ItemID.WoodenHammer)
            {
                item.useTime = 19;
                item.useAnimation = 29;
                item.hammer = 35;
            }
        }
    }
}
