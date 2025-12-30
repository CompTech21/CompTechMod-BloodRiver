using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class StarterBag : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.White;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.value = Item.buyPrice(copper: 0);
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(null, ItemID.LifeCrystal, 1);
            player.QuickSpawnItem(null, ItemID.ManaCrystal, 2);
            player.QuickSpawnItem(null, ItemID.SunMask, 1);
            player.QuickSpawnItem(null, ModContent.ItemType<BootsOfFrivolity>(), 1);
            player.QuickSpawnItem(null, ModContent.ItemType<DontDoThis>(), 1);
            player.QuickSpawnItem(null, ItemID.CorruptSeeds, 25);
            player.QuickSpawnItem(null, ItemID.CrimsonSeeds, 25);
            player.QuickSpawnItem(null, ItemID.GoldCoin, 1);
            player.QuickSpawnItem(null, ItemID.CopperHammer, 1);
            player.QuickSpawnItem(null, ItemID.LesserHealingPotion, 5);
            player.QuickSpawnItem(null, ItemID.Rope, 100);

            if (Main.getGoodWorld)
            {
                player.QuickSpawnItem(null, ItemID.ObsidianSkinPotion, 3);
                player.QuickSpawnItem(null, ItemID.InfernoPotion, 3);
                player.QuickSpawnItem(null, ItemID.Hotdog, 5);
            }
        }
    }
}
