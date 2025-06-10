using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CompTechMod.Content.Buffs;

namespace CompTechMod.Content.Items
{
    public class SunElixir : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 42;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 11);
            Item.buffType = ModContent.BuffType<SunElixirBuff>();
            Item.buffTime = 60 * 60 * 6; // 6 минут
        }

        public override bool CanUseItem(Player player)
        {
            // Не накладывает повторно, если бафф уже есть
            return !player.HasBuff(Item.buffType);
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ModContent.ItemType<LihzardEssence>(), 10)
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 10)
                .AddTile(TileID.LihzahrdFurnace)
                .Register();
        }
	}
}
