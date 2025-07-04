using Terraria;
using Terraria.ModLoader;
using CompTechMod.Content.Players;
using CompTechMod;

namespace CompTechMod.Content.NPCs
{
	public class FrostHeartGlobalNPC : GlobalNPC
	{
		public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
		{
			if (target.GetModPlayer<FrostHeartPlayer>().frostHeartEquipped)
            {
                if (Main.rand.NextFloat() < 0.25f)
                {
                    npc.AddBuff(44, 180); // Обжигающий холод на 3 сек
                }
            }
		}
	}
}
