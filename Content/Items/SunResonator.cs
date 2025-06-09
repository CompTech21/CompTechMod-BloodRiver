using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class SunResonator : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 20, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.18f;
            player.manaCost -= 0.11f;
            player.manaFlower = true;
            player.manaMagnet = true;
            player.manaRegenBonus += 5;
            player.statManaMax2 += 60;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MagnetFlower, 1)
                .AddIngredient(ModContent.ItemType<LihzardEssence>(), 30)
                .AddIngredient(ItemID.ManaCrystal, 3)
                .AddTile(TileID.LihzahrdFurnace)
                .Register();
        }
	}
}
