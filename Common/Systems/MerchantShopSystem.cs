using CompTechMod.Content.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class MerchantShopSystem : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType != NPCID.Merchant)
                return;

            shop.Add(new NPCShop.Entry(ModContent.ItemType<WornShield>(), Array.Empty<Condition>()));
        }
    }
}