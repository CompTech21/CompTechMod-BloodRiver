using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.Content.NPCs
{
    public class CreeperLootEdit : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Creeper)
            {
                // Убираем образцы ткани
                npcLoot.RemoveWhere(rule => rule is CommonDrop drop && drop.itemId == ItemID.TissueSample);
            }
        }
    }
}
