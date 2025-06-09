using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using CompTechMod.Content.Items;
using CompTechMod.DropConditions;

namespace CompTechMod.Content.NPCs
{
	public class LihzardEssenceDropGlobalNPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			int essenceType = ModContent.ItemType<LihzardEssence>();
			var condition = new LihzardUnlockedCondition();

			switch (npc.type) {
				case NPCID.Harpy:
					npcLoot.Add(ItemDropRule.ByCondition(condition, essenceType, 2, 1, 2)); // 50%, 1–2
					break;
				case NPCID.WyvernHead:
					npcLoot.Add(ItemDropRule.ByCondition(condition, essenceType, 2, 3, 5)); // 50%, 3–5
					break;
				case NPCID.MartianProbe:
					npcLoot.Add(ItemDropRule.ByCondition(condition, essenceType, 1, 10, 10)); // 100%, 10
					break;
			}
		}
	}
}
