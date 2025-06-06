using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class StarterBag : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.maxStack = 999;
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
            // Кристалл жизни (Life Crystal)
            player.QuickSpawnItem(null, ItemID.LifeCrystal, 1);

            // Кристалл маны (Mana Crystal)
            player.QuickSpawnItem(null, ItemID.ManaCrystal, 1);

            // Маска солнца (Sun Mask)
            player.QuickSpawnItem(null, ItemID.SunMask, 1);

            // Семена порчи
            player.QuickSpawnItem(null, ItemID.CorruptSeeds, 25);

            // Семена багрянца
            player.QuickSpawnItem(null, ItemID.CrimsonSeeds, 25);

            // Золотая монета
            player.QuickSpawnItem(null, ItemID.GoldCoin, 1);
        }
    }
}
