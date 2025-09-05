using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    [AutoloadEquip(EquipType.Body)]
    public class BloodyAscensionChestplate : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 60, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.defense = 45;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.21f;
            player.GetCritChance(DamageClass.Generic) += 11f;
            player.pickSpeed -= 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 150)
                .AddIngredient(ModContent.ItemType<BleedingBar>(), 40)
                .AddIngredient(ModContent.ItemType<BloodEssence>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
