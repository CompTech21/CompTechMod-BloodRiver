using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CompTechMod.Common.Systems
{
    public class EyeWorldSystem : ModSystem
    {
        public static bool SeaCreaturesEmpowered = false;
        private static bool printedMessage = false;

        public override void OnWorldLoad()
        {
            SeaCreaturesEmpowered = false;
            printedMessage = false;
        }

        public override void OnWorldUnload()
        {
            SeaCreaturesEmpowered = false;
            printedMessage = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (SeaCreaturesEmpowered)
                tag["SeaCreaturesEmpowered"] = true;

            if (printedMessage)
                tag["EyeMessagePrinted"] = true;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            SeaCreaturesEmpowered = tag.ContainsKey("SeaCreaturesEmpowered") && tag.GetBool("SeaCreaturesEmpowered");
            printedMessage = tag.ContainsKey("EyeMessagePrinted") && tag.GetBool("EyeMessagePrinted");
        }

        public override void PreUpdateWorld()
        {
            if (SeaCreaturesEmpowered && !printedMessage)
            {
                printedMessage = true;

                if (Main.netMode != NetmodeID.Server)
                {
                    Color brightCyan = new Color(0, 255, 255);
                    Main.NewText(Language.GetTextValue("Mods.CompTechMod.Messages.SeaSplinters"), brightCyan);
                }
            }
        }

        public override void PostUpdateNPCs()
        {
            if (!SeaCreaturesEmpowered && NPC.downedBoss1) // Eye of Cthulhu defeated
            {
                SeaCreaturesEmpowered = true;
            }
        }
    }
}
