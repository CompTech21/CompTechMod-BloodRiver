using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CompTechMod.Common.Systems;

namespace CompTechMod.Content.Items
{
    public class Strawberry : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item4;
            Item.rare = ItemRarityID.Yellow;
            Item.consumable = true;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 10, 0, 0);
        }

        public override bool CanUseItem(Player player)
        {
            return !StrawberryModPlayer.HasEaten(player) && player.statLifeMax >= 500;
        }

        public override bool? UseItem(Player player)
        {
            if (!StrawberryModPlayer.HasEaten(player))
            {
                StrawberryModPlayer.SetEaten(player, true);

                // Эффект
                CombatText.NewText(player.Hitbox, Color.LightPink, "+25 HP!", true);
                player.statLife += 25;

                return true;
            }

            return false;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LifeFruit, 5)
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddIngredient(ItemID.Ectoplasm, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}