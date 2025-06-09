using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;

namespace CompTechMod.Content.NPCs
{
    public class QueenBeeCloneSystem : GlobalNPC
    {
        private bool isClone = false;
        private int linkedOriginal = -1;

        public override bool InstancePerEntity => true;

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.QueenBee && !isClone && npc.life < npc.lifeMax * 0.3f && !AlreadySpawnedClone())
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int index = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X + 100, (int)npc.Center.Y, NPCID.QueenBee);
                    NPC clone = Main.npc[index];

                    if (clone.TryGetGlobalNPC(out QueenBeeCloneSystem cloneData))
                    {
                        cloneData.isClone = true;
                        cloneData.linkedOriginal = npc.whoAmI;
                    }

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
            }

            if (isClone && (!Main.npc[linkedOriginal].active || Main.npc[linkedOriginal].type != NPCID.QueenBee))
            {
                npc.active = false;
                npc.life = 0;
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (npc.type == NPCID.QueenBee && isClone)
            {
                npc.life = 0;
                npc.active = false;
                return false;
            }
            return base.CheckDead(npc);
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.QueenBee && isClone)
            {
                
            }
        }

        private bool AlreadySpawnedClone()
        {
            int count = 0;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == NPCID.QueenBee)
                    count++;
            }
            return count > 1;
        }

        public override void OnKill(NPC npc)
        {
            // Удаляем клонов после убийства оригинала (на всякий случай)
            if (npc.type == NPCID.QueenBee && !isClone)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == NPCID.QueenBee && Main.npc[i].TryGetGlobalNPC(out QueenBeeCloneSystem otherData) && otherData.isClone)
                    {
                        Main.npc[i].active = false;
                        Main.npc[i].life = 0;
                    }
                }
            }
        }
    }
}
