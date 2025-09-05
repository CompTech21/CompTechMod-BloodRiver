using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Tiles;

namespace CompTechMod.Content.Items
{
    public class BloodyAltarItem : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 0;
            Item.createTile = ModContent.TileType<BloodyAltar>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BleedingBar>(), 10);
            recipe.AddIngredient(ItemID.SpookyWood, 25);
            recipe.AddTile(TileID.LunarCraftingStation); // Ancient Manipulator
            recipe.Register();
        }
    }
}
