using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CompTechMod.Content.Items
{
    public class BloodEssence : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(copper: 81);
            Item.rare = ItemRarityID.Red;
            Item.material = true;
        }
        
        public override void Update(ref float gravity, ref float maxFallSpeed) {
			gravity = 0f;
			maxFallSpeed = 0f;

			if (Item.velocity.Y != 0f) {
				Item.velocity.Y = 0f;
			}
			if (Item.velocity.X != 0f) {
				Item.velocity.X *= 0.95f;
				if (Item.velocity.X is > -0.1f and < 0.1f)
					Item.velocity.X = 0f;
			}
		}
    }
}