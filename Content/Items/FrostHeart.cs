using CompTechMod.Content.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
	public class FrostHeart : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 22;
			Item.accessory = true;
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
			Item.value = Item.sellPrice(gold: 5);
			Item.defense = 3;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// Иммунитет к дебаффам
			player.buffImmune[46] = true; // Переохлаждение (Chilled)
			player.buffImmune[47] = true; // Замёрзший (Frozen)

			// Увеличение скорости ближнего боя
			player.GetAttackSpeed(DamageClass.Melee) += 0.06f;

			// Включаем наш эффект при получении урона
			player.GetModPlayer<FrostHeartPlayer>().frostHeartEquipped = true;
		}
	}
}
