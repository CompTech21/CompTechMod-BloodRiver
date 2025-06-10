using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
	public class SeaSplinter : ModItem
	{

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.value = Item.buyPrice(silver: 7);
			Item.rare = ItemRarityID.Green;
			Item.material = true;
		}
	}
}
