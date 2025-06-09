using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class MasteryEmblem : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 14);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Урон
            player.GetDamage(DamageClass.Melee) += 0.18f;
            player.GetDamage(DamageClass.Ranged) += 0.18f;
            player.GetDamage(DamageClass.Magic) += 0.18f;
            player.GetDamage(DamageClass.Summon) += 0.18f;

            // Крит шанс
            player.GetCritChance(DamageClass.Melee) += 14f;
            player.GetCritChance(DamageClass.Ranged) += 14f;
            player.GetCritChance(DamageClass.Magic) += 14f;
            player.GetCritChance(DamageClass.Summon) += 14f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PrecisionEmblem>(), 1);
            recipe.AddIngredient(ItemID.SoulofSight, 20);
            recipe.AddIngredient(ItemID.FragmentStardust, 25);
            recipe.AddTile(TileID.LunarCraftingStation); // Ancient Manipulator
            recipe.Register();
        }
    }
}
