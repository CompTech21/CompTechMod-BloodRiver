using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Common.Systems;

namespace CompTechMod.Content.Items
{
    public class DontDoThis : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.consumable = true;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Red;
        }

        public override bool? UseItem(Player player)
        {
            CompWorld.DontDoThisMode = true;
            if (Main.netMode != NetmodeID.Server)
                Main.NewText("DontDoThis Mode Activated!", 255, 50, 50);
            return true;
        }
    }
}
