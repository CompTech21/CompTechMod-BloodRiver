using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CompTechMod.Content.Buffs;

namespace CompTechMod.Content.Items
{
    public class SoulGem : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<PhantomPactPlayer>();
            modPlayer.hasPhantomPact = true;

            player.GetDamage(DamageClass.Generic) += 1.20f;
        }

        public override void UpdateEquip(Player player)
        {
            if (player.statLifeMax2 > 1)
            {
                player.statLifeMax2 = 1;
            }
        }
    }
}