using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class BloodyLightningPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 450;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 9f;
            Item.value = Item.sellPrice(gold: 90);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.pick = 1000; // Мощность кирки
            Item.tileBoost = 10; // Бонус к дальности
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.NebulaPickaxe, 1)
                .AddIngredient(ItemID.LaserDrill, 1)
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 150)
                .AddIngredient(ItemID.LunarBar, 20)
                .AddIngredient(ModContent.ItemType<BleedingBar>(), 20)
                .AddIngredient(ModContent.ItemType<BloodEssence>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
