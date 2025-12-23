using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Buffs;
using CompTechMod.Common.Players;
using CompTechMod.Common.Keybinds;
using System.Linq;



namespace CompTechMod.Content.Items
{
    public class BloodCarapace : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(platinum: 1, gold: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BloodCarapacePlayer>().hasCarapaceEquipped = true;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips)
        {
            string key = CompTechKeybinds.BloodCarapaceKey.GetAssignedKeys().FirstOrDefault();

            if (string.IsNullOrEmpty(key))
                key = "Unbound";

            foreach (var line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    line.Text = line.Text.Replace("{0}", key);
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FrozenShield, 1);
            recipe.AddIngredient(ItemID.TurtleShell, 3);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ModContent.ItemType<BleedingBar>(), 10);
            recipe.AddTile(TileID.LunarCraftingStation); // Ancient Manipulator
            recipe.Register();
        }
    }
}
