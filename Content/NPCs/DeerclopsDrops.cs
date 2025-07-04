using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.Content.NPCs
{
    public class DeerclopsDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot loot)
        {
            if (npc.type == NPCID.Deerclops)
	        {
		        loot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Items.FrostHeart>()));
	        }
        }
    }
}