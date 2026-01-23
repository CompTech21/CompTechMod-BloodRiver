using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class Rom : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 48;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 3, silver: 40);

            Item.buffType = ModContent.BuffType<Buffs.RomBuff>();
            Item.buffTime = 60 * 60 * 6; // 6 минут
        }

        public override bool CanUseItem(Player player)
        {
            // Чтобы не стакался
            return !player.HasBuff(Item.buffType);
        }
    }
}