using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    [AutoloadEquip(EquipType.Legs)]
    public class BloodyAscensionBoots : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 55, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.defense = 35;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.18f;
            player.GetCritChance(DamageClass.Generic) += 11f;
            player.moveSpeed += 0.30f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 150)
                .AddIngredient(ModContent.ItemType<BleedingBar>(), 30)
                .AddIngredient(ModContent.ItemType<BloodEssence>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
