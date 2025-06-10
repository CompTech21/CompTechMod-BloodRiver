using Terraria;
using Terraria.ModLoader;

namespace CompTechMod.Content.Buffs
{
	public class SunElixirBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 2; // +2 к регенерации хп
			player.moveSpeed += 0.08f; // +8% к скорости передвижения
			player.wingTimeMax = (int)(player.wingTimeMax * 1.25f); // +25% к длительности полёта
		}
	}
}
