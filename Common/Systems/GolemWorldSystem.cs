using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.Chat;
using CompTechMod.Common.DropConditions;
using Terraria.Net;

namespace CompTechMod.Common.Systems
{
    public class GolemWorldSystem : ModSystem
    {
        public static bool LihzardUnlocked { get; private set; }
        private bool messagePrinted;

        public override void OnWorldLoad()
        {
            if (NPC.downedGolemBoss)
            {
                LihzardUnlocked = true;
            }

            messagePrinted = false;
        }

        public override void OnWorldUnload()
        {
            LihzardUnlocked = false;
            messagePrinted = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["LihzardUnlocked"] = LihzardUnlocked;
            tag["GolemMessagePrinted"] = messagePrinted;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            LihzardUnlocked = tag.ContainsKey("LihzardUnlocked") && tag.GetBool("LihzardUnlocked");
            messagePrinted = tag.ContainsKey("GolemMessagePrinted") && tag.GetBool("GolemMessagePrinted");
        }

        public override void PostUpdateNPCs()
        {
            // Проверяем, был ли убит голем впервые
            if (!LihzardUnlocked && NPC.downedGolemBoss)
            {
                LihzardUnlocked = true;
                PrintMessage();
            }
        }

        private void PrintMessage()
        {
            if (messagePrinted) return;
            messagePrinted = true;

            Color color = new Color(255, 185, 23); // солнечный оттенок
            string text = Language.GetTextValue("Mods.CompTechMod.Messages.SolarDeityDeath");

            // 🌐 Сервер рассылает сообщение всем клиентам
            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            }
            else
            {
                // одиночка
                Main.NewText(text, color);
            }
        }
    }
}
