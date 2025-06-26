using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CompTechMod.Common.Players
{
    public class EnvironmentDebuffPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            // Снежный биом → Переохлаждение (Frostburn / ID: 46)
            if (Player.ZoneSnow)
            {
                Player.AddBuff(46, 2); // 2 тика = 1/30 сек. Обновляется постоянно
            }

            // Ад → В огне (On Fire! / ID: 24)
            if (Player.ZoneUnderworldHeight)
            {
                Player.AddBuff(24, 2);
            }
        }
    }
}
