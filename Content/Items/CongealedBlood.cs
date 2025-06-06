using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CompTechMod.Content.Items
{
    public class CongealedBlood : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(copper: 25);
            Item.rare = ItemRarityID.Green;
        }
    }
}