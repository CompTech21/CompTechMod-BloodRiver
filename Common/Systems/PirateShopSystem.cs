using CompTechMod.Content.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class PirateShopSystem : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType != NPCID.Pirate)
                return;

            shop.Add(new NPCShop.Entry(ModContent.ItemType<Rom>(), Array.Empty<Condition>()));
        }
    }
}