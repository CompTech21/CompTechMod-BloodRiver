using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CompTechMod.Common.Systems
{
	public class GoblinsSystem : ModSystem
	{
		private static bool goblinChecked = false;

		public override void OnWorldLoad() {
			goblinChecked = false;
		}

		public override void OnWorldUnload() {
			goblinChecked = false;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (goblinChecked)
				tag["GoblinEngineerChecked"] = true;
		}

		public override void LoadWorldData(TagCompound tag) {
			goblinChecked = tag.ContainsKey("GoblinEngineerChecked") && tag.GetBool("GoblinEngineerChecked");
		}

		public override void PostUpdateWorld()
		{
			if (!goblinChecked && NPC.downedGoblins)
			{
				goblinChecked = true;

				if (!NPC.AnyNPCs(NPCID.GoblinTinkerer))
				{
					// Принудительный спавн гоблина на позиции игрока
					int x = (int)(Main.spawnTileX * 16);
					int y = (int)(Main.spawnTileY * 16);
					int index = NPC.NewNPC(null, x, y, NPCID.GoblinTinkerer);

					if (index < Main.maxNPCs)
					{
						Main.npc[index].homeless = true; // Обозначить, что у него нет дома
						Main.npc[index].direction = 1;
						Main.npc[index].netUpdate = true;
					}
				}
			}
		}
	}
}
