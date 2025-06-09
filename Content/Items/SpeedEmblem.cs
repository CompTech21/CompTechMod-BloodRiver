using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace CompTechMod.Content.Items
{
    public class SpeedEmblem : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.16f; // +16% скорость передвижения
            player.GetAttackSpeed(DamageClass.Melee) += 0.08f; // +8% скорость атаки ближнего боя
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HermesBoots, 1);
            recipe.AddIngredient(ItemID.JungleSpores, 15);
            recipe.AddIngredient(ItemID.Vine, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
