using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
	public class LihzardEssence : ModItem
	{
		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(silver: 86);
			Item.rare = ItemRarityID.Yellow;
			Item.material = true;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed) {
			// Полностью отключаем гравитацию
			gravity = 0f;
			maxFallSpeed = 0f;

			// Принудительно замираем предмет в воздухе
			if (Item.velocity.Y != 0f) {
				Item.velocity.Y = 0f;
			}
			if (Item.velocity.X != 0f) {
				Item.velocity.X *= 0.95f; // слегка затухает горизонтальное движение, если есть
				if (Item.velocity.X is > -0.1f and < 0.1f)
					Item.velocity.X = 0f;
			}
		}
	}
}
