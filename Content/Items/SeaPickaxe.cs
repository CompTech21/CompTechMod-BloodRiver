using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class SeaPickaxe : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.pick = 60;
            Item.knockBack = 3f;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(silver: 60);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.tileBoost = 0; // не увеличивает дистанцию добычи
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SeaSplinter>(), 10);
            recipe.AddRecipeGroup("CompTechMod:SilverBar", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
