using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.Content.NPCs
{
    public class DungeonSpiritDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot loot)
        {
            if (npc.type == NPCID.DungeonSpirit)
            {
                loot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.SoulGem>(), 20));
            }
        }
    }
}