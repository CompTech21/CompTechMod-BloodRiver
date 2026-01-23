using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class BloodySmoothie : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsFood[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 38;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;

            Item.buffType = BuffID.WellFed2;
            Item.buffTime = 60 * 60 * 3; // 3 минуты

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ItemID.Vertebrae, 2)
                .AddIngredient(ItemID.Deathweed, 1)
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 3)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
