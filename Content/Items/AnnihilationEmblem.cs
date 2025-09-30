using CompTechMod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class AnnihilationEmblem : ModItem
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
            // Общий урон
            player.GetDamage(DamageClass.Melee) += 0.30f;
            player.GetDamage(DamageClass.Ranged) += 0.30f;
            player.GetDamage(DamageClass.Magic) += 0.30f;
            player.GetDamage(DamageClass.Summon) += 0.30f;

            // Пробивание
            player.GetArmorPenetration(DamageClass.Generic) += 15;

            // Крит. шанс
            player.GetCritChance(DamageClass.Melee) += 19f;
            player.GetCritChance(DamageClass.Ranged) += 19f;
            player.GetCritChance(DamageClass.Magic) += 19f;
            player.GetCritChance(DamageClass.Summon) += 19f;

            // Скорость атаки
            player.GetAttackSpeed(DamageClass.Melee) += 0.17f;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.17f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.17f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SupremacyEmblem>());
            recipe.AddIngredient(ModContent.ItemType<MasteryEmblem>());
            recipe.AddIngredient(ItemID.FragmentSolar, 25);
            recipe.AddIngredient(ItemID.FragmentVortex, 25);
            recipe.AddIngredient(ItemID.FragmentNebula, 25);
            recipe.AddIngredient(ItemID.FragmentStardust, 25);
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
