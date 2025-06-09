using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class PrecisionEmblem : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 4);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // +12% урон ко всем видам
            player.GetDamage(DamageClass.Melee) += 0.18f;
            player.GetDamage(DamageClass.Ranged) += 0.18f;
            player.GetDamage(DamageClass.Magic) += 0.18f;
            player.GetDamage(DamageClass.Summon) += 0.18f;

            // +15% крит шанс ко всем типам
            player.GetCritChance(DamageClass.Melee) += 15f;
            player.GetCritChance(DamageClass.Ranged) += 15f;
            player.GetCritChance(DamageClass.Magic) += 15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DestroyerEmblem);
            recipe.AddIngredient(ItemID.EyeoftheGolem);
            recipe.AddIngredient(ModContent.ItemType<LihzardEssence>(), 30);
            recipe.AddTile(TileID.LihzahrdFurnace);
            recipe.Register();
        }
    }
}
