using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Common.Systems;

namespace CompTechMod.Content.Items
{
    public class BloodyRelic : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 22;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Roar;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return !BossRushSystem.Active;
        }

        public override bool? UseItem(Player player)
        {
            BossRushSystem.Start(player);
            return true;
        }
    }
}
