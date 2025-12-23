using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.Chat;
using Terraria.Net;

namespace CompTechMod.Common.Systems
{
    public class EyeWorldSystem : ModSystem
    {
        public bool SeaCreaturesEmpowered;
        private bool messagePrinted;

        public override void OnWorldLoad()
        {
            // При загрузке мира проверяем, был ли убит глаз
            if (NPC.downedBoss1)
            {
                SeaCreaturesEmpowered = true;
            }

            messagePrinted = false; // Сбрасываем флаг для корректного отображения
        }

        public override void OnWorldUnload()
        {
            SeaCreaturesEmpowered = false;
            messagePrinted = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["SeaCreaturesEmpowered"] = SeaCreaturesEmpowered;
            tag["EyeMessagePrinted"] = messagePrinted;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            SeaCreaturesEmpowered = tag.ContainsKey("SeaCreaturesEmpowered") && tag.GetBool("SeaCreaturesEmpowered");
            messagePrinted = tag.ContainsKey("EyeMessagePrinted") && tag.GetBool("EyeMessagePrinted");
        }

        public override void PostUpdateNPCs()
        {
            // Проверяем, был ли убит глаз впервые
            if (!SeaCreaturesEmpowered && NPC.downedBoss1)
            {
                SeaCreaturesEmpowered = true;
                PrintMessage();
            }
        }

        private void PrintMessage()
        {
            if (messagePrinted) return;
            messagePrinted = true;

            Color color = new Color(0, 255, 255);
            string text = Language.GetTextValue("Mods.CompTechMod.Messages.SeaSplinters");

            // 🌐 Мультиплеер: сервер рассылает всем клиентам
            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            }
            else
            {
                // Одиночка: просто выводим текст
                Main.NewText(text, color);
            }
        }
    }
}
