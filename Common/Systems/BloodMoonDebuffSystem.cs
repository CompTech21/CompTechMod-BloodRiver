using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class BloodMoonDebuffSystem : ModSystem
    {
        public override void PostUpdatePlayers()
        {
            if (!Main.bloodMoon)
                return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];

                if (!player.active || player.dead)
                    continue;

                // слепота
                player.AddBuff(BuffID.Darkness, 2);

                // слабость
                player.AddBuff(BuffID.Weak, 2);

                // кровотечение
                player.AddBuff(BuffID.Bleeding, 2);
            }
        }
    }
}
