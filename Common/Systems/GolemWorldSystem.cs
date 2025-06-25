using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CompTechMod.Common.Systems
{
	public class GolemWorldSystem : ModSystem
	{
		public static bool LihzardUnlocked = false;
		private static bool printedMessage = false;

		public override void OnWorldLoad() {
			LihzardUnlocked = false;
			printedMessage = false;
		}

		public override void OnWorldUnload() {
			LihzardUnlocked = false;
			printedMessage = false;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (LihzardUnlocked)
				tag["LihzardUnlocked"] = true;

			if (printedMessage)
				tag["GolemMessagePrinted"] = true;
		}

		public override void LoadWorldData(TagCompound tag) {
			LihzardUnlocked = tag.ContainsKey("LihzardUnlocked") && tag.GetBool("LihzardUnlocked");
			printedMessage = tag.ContainsKey("GolemMessagePrinted") && tag.GetBool("GolemMessagePrinted");
		}

		public override void PreUpdateWorld() {
			if (LihzardUnlocked && !printedMessage) {
				printedMessage = true;

				if (Main.netMode != NetmodeID.Server) {
					Main.NewText(Language.GetTextValue("Mods.CompTechMod.Messages.SolarDeityDeath"), new Color(255, 185, 23));
				}
			}
		}

		public override void PostUpdateNPCs() {
			if (!LihzardUnlocked && NPC.downedGolemBoss) {
				LihzardUnlocked = true;
			}
		}
	}
}
