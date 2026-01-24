using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace CompTechMod.Content.Items
{
    public class MightEmblem : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 4);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.14f;       // +9% урон
            player.GetKnockback(DamageClass.Melee) += 1f;       // +сильное отбрасывание
            player.GetAttackSpeed(DamageClass.Melee) += 0.15f;  // +15% скорость атаки
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PowerGlove); // Перчатка силы
            recipe.AddIngredient(ItemID.UnicornHorn, 5);
            recipe.AddRecipeGroup("CompTechMod:CobaltBar", 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
