using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using CompTechMod.Content.Items;

namespace CompTechMod.Common.NPCs
{
    public class DreadnautilusDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BloodNautilus)
            {
                npcLoot.Add(
                    ItemDropRule.Common(
                        ModContent.ItemType<Content.Items.BloodShell>(),
                        1
                    )
                );
            }
        }
    }
}
