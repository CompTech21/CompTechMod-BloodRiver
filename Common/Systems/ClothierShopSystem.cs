using CompTechMod.Content.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class ClothierShopSystem : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType != NPCID.Clothier)
                return;

            shop.Add(new NPCShop.Entry(ModContent.ItemType<PrecisionBelt>(), Condition.DownedSkeletron));
        }
    }
}
