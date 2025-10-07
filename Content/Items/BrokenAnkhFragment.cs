using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CompTechMod.Content.Items
{
    public class BrokenAnkhFragment : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(copper: 0);
            Item.rare = ItemRarityID.White;
        }
    }
}