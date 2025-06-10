using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using CompTechMod.Common.Systems;

namespace CompTechMod
{
	public class CompTechMod : Mod
	{
		public override void PostSetupContent()
		{
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				bossChecklist.Call(
					"AddBoss",
					this,
					"Deep Sea Shark",
					ModContent.NPCType<Content.NPCs.DeepSeaShark>(),
					0.25f, // после Eye of Cthulhu (у него 0.2)
					() => NPC.downedBoss1,
					() => CompTechModSystem.downedDeepSeaShark,
					new string[]
					{
						"Spawned rarely in the ocean after Eye of Cthulhu is defeated."
					},
					ModContent.Request<Texture2D>("CompTechMod/Content/NPCs/DeepSeaShark").Value
				);
			}
		}
	}
}
