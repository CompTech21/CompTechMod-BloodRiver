using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
	public class BootsOfFrivolity : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 28;
			Item.accessory = true;
			Item.rare = ItemRarityID.White; // Белая редкость
			Item.value = Item.sellPrice(copper: 14); // 14 медных
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.accRunSpeed = 3.7f; // ≈ 16.1 mph ≈ 26 км/ч
			// Никаких moveSpeed прибавок — иначе скорость станет выше
		}
	}
}
