using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Common.Players;
using Terraria.Localization;


namespace CompTechMod.Content.Items
{
    [AutoloadEquip(EquipType.Head)]
    public class BloodyAscensionHelmet : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.defense = 30;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.18f;
            player.GetCritChance(DamageClass.Generic) += 15f;
            player.maxMinions += 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BloodyAscensionChestplate>() &&
                   legs.type == ModContent.ItemType<BloodyAscensionBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.CompTechMod.SetBonuses.BloodyAscension");
            player.GetModPlayer<BloodyAscensionPlayer>().bloodySet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 150)
                .AddIngredient(ModContent.ItemType<BleedingBar>(), 25)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
