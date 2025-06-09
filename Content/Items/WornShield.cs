using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Players;

namespace CompTechMod.Content.Items
{
    public class WornShield : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.value = Item.buyPrice(silver: 98);
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
            Item.defense = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<WornShieldDashPlayer>().DashAccessoryEquipped = true;
        }
    }
}
