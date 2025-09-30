using System;
using CompTechMod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class ShopSystem : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            // Торговец
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(new NPCShop.Entry(ModContent.ItemType<WornShield>(), Array.Empty<Condition>()));
                shop.Add(ItemID.WormholePotion, Array.Empty<Condition>());
                shop.Add(ItemID.BottledWater, Array.Empty<Condition>());
            }

            // Дриада
            if (shop.NpcType == NPCID.Dryad)
            {
                shop.Add(ItemID.JungleRose, Array.Empty<Condition>());
            }

            if (shop.NpcType == NPCID.Wizard)
            {
                shop.Add(ItemID.RodofDiscord, Array.Empty<Condition>());
            }
        }
    }
}
