using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CompTechMod.Common.Systems;
using Terraria.DataStructures;

namespace CompTechMod.Common.GlobalNPCs
{
    public class BossRushGlobalNPC : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (!BossRushSystem.Active)
                return;

            // Усиление боссов
            if (npc.boss ||
                npc.type == NPCID.EaterofWorldsHead ||
                npc.type == NPCID.EaterofWorldsBody ||
                npc.type == NPCID.EaterofWorldsTail ||
                npc.type == NPCID.BrainofCthulhu)
            {
                npc.lifeMax *= 20;
                npc.life = npc.lifeMax;
            }
        }

        public override bool CheckActive(NPC npc)
        {
            if (!BossRushSystem.Active)
                return true;

            // Симулируем "правильный биом" для EoW и Brain of Cthulhu
            if (npc.type == NPCID.EaterofWorldsHead ||
                npc.type == NPCID.EaterofWorldsBody ||
                npc.type == NPCID.EaterofWorldsTail ||
                npc.type == NPCID.BrainofCthulhu)
            {
                npc.timeLeft = 750; // сброс таймера деспавна
                return false; // запрещаем стандартное отключение активности
            }

            return true;
        }

        public override void OnKill(NPC npc)
        {
            BossRushSystem.OnBossKilled(npc);
        }
    }
}
