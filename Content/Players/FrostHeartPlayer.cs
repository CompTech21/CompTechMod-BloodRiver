using CompTechMod;
using Terraria;
using Terraria.ModLoader;
using CompTechMod.Content.Players;


namespace CompTechMod.Content.Players
{
	public class FrostHeartPlayer : ModPlayer
	{
		public bool frostHeartEquipped;

		public override void ResetEffects()
		{
			frostHeartEquipped = false;
		}
	}
}
