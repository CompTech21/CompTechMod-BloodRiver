using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.Content.NPCs
{
    public class CreeperLootEdit : GlobalNPC
    {
                public override void OnKill(NPC npc)
                {
                    if (npc.type == NPCID.Creeper)
                    {
                        npc.value = 0;

                        for (int i = 0; i < Main.item.Length; i++)
                        {
                            Item item = Main.item[i];
                            if (item.active && (item.type == ItemID.CrimtaneOre || item.type == ItemID.TissueSample))
                            {
                                item.active = false;
                            }
                }
            }
        }
    }
}