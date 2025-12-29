using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace CompTechMod.Content.Items
{
    [AutoloadEquip(EquipType.Wings)]
    public class ScarletWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(
                int.MaxValue, // формально бесконечный полёт
                22f,
                1.8f
            );
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(platinum: 1);
        }

        // 🔥 КЛЮЧЕВОЙ ФИКС
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // ботинки больше НЕ смогут жрать выносливость
            player.wingTime = player.wingTimeMax;
        }

        public override void VerticalWingSpeeds(
            Player player,
            ref float ascentWhenFalling,
            ref float ascentWhenRising,
            ref float maxCanAscendMultiplier,
            ref float maxAscentMultiplier,
            ref float constantAscend
        )
        {
            ascentWhenFalling = 1.15f;
            ascentWhenRising = 0.55f;
            maxCanAscendMultiplier = 1.6f;
            maxAscentMultiplier = 2.2f;
            constantAscend = 0.28f;
        }

        public override void HorizontalWingSpeeds(
            Player player,
            ref float speed,
            ref float acceleration
        )
        {
            speed = 23f;
            acceleration = 0.75f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CreativeWings);
            recipe.AddIngredient(ItemID.FrozenWings);
            recipe.AddIngredient(ItemID.BeeWings);
            recipe.AddIngredient(ItemID.MothronWings);
            recipe.AddIngredient(ItemID.FestiveWings);
            recipe.AddIngredient(ItemID.SpookyWings);
            recipe.AddIngredient(ItemID.RainbowWings);
            recipe.AddIngredient(ItemID.FishronWings);
            recipe.AddIngredient(ItemID.WingsStardust);
            recipe.AddIngredient(ItemID.EmpressFlightBooster);
            recipe.AddIngredient(ModContent.ItemType<BleedingBar>(), 20);
            recipe.AddIngredient(ModContent.ItemType<CongealedBlood>(), 150);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
