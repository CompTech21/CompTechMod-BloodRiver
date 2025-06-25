using CompTechMod.Content.Items;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ID;
using CompTechMod.Common.DropConditions;


namespace CompTechMod.Content.NPCs
{
    public class SeaSplinterGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            int seaSplinter = ModContent.ItemType<SeaSplinter>();

            // Условие: только после победы над Глазом Ктулху
            IItemDropRuleCondition condition = new AfterEyeOfCthulhuCondition();

            // Розовая медуза (Pink Jellyfish)
            if (npc.type == NPCID.PinkJellyfish)
            {
                npcLoot.Add(ItemDropRule.ByCondition(condition, seaSplinter, 2, 1, 2)); // 50%, 1–2
            }

            // Краб (Crab)
            else if (npc.type == NPCID.Crab)
            {
                npcLoot.Add(ItemDropRule.ByCondition(condition, seaSplinter, 2, 1, 2)); // 50%, 1–2
            }

            // Акула (Shark)
            else if (npc.type == NPCID.Shark)
            {
                npcLoot.Add(ItemDropRule.ByCondition(condition, seaSplinter, 1, 2, 2)); // 100%, 2
            }

            // Кальмар (Squid)
            else if (npc.type == NPCID.Squid)
            {
                npcLoot.Add(ItemDropRule.ByCondition(condition, seaSplinter, 1, 3, 3)); // 100%, 3
            }

            // Морская улитка (Sea Snail)
            else if (npc.type == NPCID.SeaSnail)
            {
                npcLoot.Add(ItemDropRule.ByCondition(condition, seaSplinter, 1, 5, 5)); // 100%, 5
            }

        }

        public override bool InstancePerEntity => true;
    }
}
