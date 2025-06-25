using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Events;

namespace CompTechMod.Common.Players
{
    public class RespawnTimePlayer : ModPlayer
    {
        private bool customRespawnApplied = false;

        public override void UpdateDead()
        {
            if (!customRespawnApplied)
            {
                bool bossAlive = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].boss)
                    {
                        bossAlive = true;
                        break;
                    }
                }

                if (bossAlive)
                {
                    Player.respawnTimer = 60 * 15; // 15 сек при боссе
                }
                else if (Main.invasionType != 0 || Main.eclipse || Main.bloodMoon || DD2Event.Ongoing)
                {
                    Player.respawnTimer = 60 * 7; // 7 сек при событии
                }
                else
                {
                    Player.respawnTimer = 60 * 3; // 3 сек по умолчанию
                }

                customRespawnApplied = true;
            }
        }

        public override void OnRespawn()
        {
            // Сброс флага после возрождения
            customRespawnApplied = false;
        }
    }
}
