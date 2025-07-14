using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class BleedingBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Название и описание задаются через локализацию
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.consumable = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BleedingOreItem>(), 20);
            recipe.AddTile(TileID.LunarCraftingStation); // Ancient Manipulator
            recipe.Register();
        }
    }
}