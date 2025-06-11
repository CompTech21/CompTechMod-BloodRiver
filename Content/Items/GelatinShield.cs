using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Players;

namespace CompTechMod.Content.Items
{
    public class GelatinShield : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 38;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GelatinShieldDashPlayer>().DashAccessoryEquipped = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GelBalloon, 30);
            recipe.AddIngredient(ItemID.SoulofLight, 15);
            recipe.AddIngredient(ItemID.PinkGel, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
