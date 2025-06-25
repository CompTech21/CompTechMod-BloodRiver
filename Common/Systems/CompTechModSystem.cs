using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CompTechMod.Common.Systems
{
	public class CompTechModSystem : ModSystem
	{

		
		public static bool downedDeepSeaShark;

		public override void OnWorldLoad()
		{
			downedDeepSeaShark = false;
		}

		public override void OnWorldUnload()
		{
			downedDeepSeaShark = false;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			if (downedDeepSeaShark)
				tag["downedDeepSeaShark"] = true;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			downedDeepSeaShark = tag.ContainsKey("downedDeepSeaShark") && tag.GetBool("downedDeepSeaShark");
		}
	}
}
