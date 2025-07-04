using Terraria;
using Terraria.ModLoader;
using CompTechMod.Common.Configs;

namespace CompTechMod.Common.Global
{
    public class BossAndMobHealth : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {
            var config = ModContent.GetInstance<DifficultyConfig>();

            if (npc.boss && config.IncreaseBossHealth)
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5f);
                npc.life = npc.lifeMax;
            }
            else if (!npc.boss && !npc.friendly && !npc.townNPC && config.IncreaseMobHealth)
            {
                npc.lifeMax = npc.lifeMax * 2;
                npc.life = npc.lifeMax;
            }
        }
    }
}