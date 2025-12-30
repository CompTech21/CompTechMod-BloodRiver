using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using CompTechMod.Content.Items;

namespace CompTechMod.Common.Global
{
    public class MoonlordDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.BloodyRelic>(), 10)); // 10%
            }
        }
    }
}
