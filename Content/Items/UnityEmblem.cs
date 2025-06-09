using CompTechMod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class UnityEmblem : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 35);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.15f; // Снижение получаемого урона
            player.statLifeMax2 += 30; // Увеличение хп
            player.statDefense += 12; // Защита
            player.moveSpeed += 0.33f; // Скорость передвижения
            player.wingTimeMax = (int)(player.wingTimeMax * 1.45f); // Время полёта

            // Скорость атаки
            player.GetAttackSpeed(DamageClass.Melee) += 0.19f;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.19f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.19f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TenacityEmblem>(), 1);
            recipe.AddIngredient(ModContent.ItemType<DynamicEmblem>(), 1);
            recipe.AddIngredient(ItemID.FragmentVortex, 25);
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
