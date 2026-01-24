using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CompTechMod.Common.Systems;

namespace CompTechMod.Content.Items
{
    public class SecurityEmblem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 1, silver: 65);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 6;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("CompTechMod:IronBar", 10);
            recipe.AddRecipeGroup("CompTechMod:EvilBar", 10);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
