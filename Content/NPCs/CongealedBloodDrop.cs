using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.Content.NPCs
{
    public class CongealedBloodDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            int itemType = ModContent.ItemType<Items.CongealedBlood>();

            switch (npc.type)
            {
                case NPCID.BloodZombie:
                case NPCID.Drippler:
                    npcLoot.Add(ItemDropRule.Common(itemType, 2)); // 50% шанс (1 из 2)
                    break;

                case NPCID.EyeballFlyingFish:
                    npcLoot.Add(ItemDropRule.Common(itemType, 1, 2, 5)); // 100%, 2-4 штук
                    break;

                case NPCID.GoblinShark:
                    npcLoot.Add(ItemDropRule.Common(itemType, 1, 5, 8)); // 100%, 5-7 штук
                    break;

                case NPCID.BloodNautilus:
                    npcLoot.Add(ItemDropRule.Common(itemType, 1, 30, 36)); // 100%, 30-35 штук
                    break;
            }
        }
    }
}