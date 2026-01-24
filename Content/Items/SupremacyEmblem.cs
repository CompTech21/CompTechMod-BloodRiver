using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class SupremacyEmblem : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 11);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Урон
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.GetDamage(DamageClass.Summon) += 0.15f;

            // Пробивание
            player.GetArmorPenetration(DamageClass.Generic) += 15;

            // Крит шанс
            player.GetCritChance(DamageClass.Melee) += 11f;
            player.GetCritChance(DamageClass.Ranged) += 11f;
            player.GetCritChance(DamageClass.Magic) += 11f;
            player.GetCritChance(DamageClass.Summon) += 11f;

            // Скорость атаки
            player.GetAttackSpeed(DamageClass.Melee) += 0.13f;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.13f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.13f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MightEmblem>(), 1);
            recipe.AddIngredient(ItemID.SunStone, 1);
            recipe.AddIngredient(ItemID.FragmentSolar, 25);
            recipe.AddIngredient(ItemID.GoldCrown, 1);
            recipe.AddTile(TileID.LunarCraftingStation); // Ancient Manipulator
            recipe.Register();
        }
    }
}
