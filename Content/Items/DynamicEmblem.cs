using CompTechMod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class DynamicEmblem : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 9);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.29f;
            player.wingTimeMax = (int)(player.wingTimeMax * 1.25f);

            player.GetAttackSpeed(DamageClass.Melee) += 0.17f;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.17f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.17f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SpeedEmblem>(), 1);
            recipe.AddIngredient(ItemID.FragmentVortex, 25);
            recipe.AddIngredient(ItemID.Feather, 25);
            recipe.AddRecipeGroup("CompTechMod:Wings");
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
