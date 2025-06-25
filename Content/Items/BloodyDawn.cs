using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class BloodyDawn : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 325;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 3;
            Item.useTime = 3;
            Item.shootSpeed = 16f;
            Item.knockBack = 6f;
            Item.width = 30;
            Item.height = 26;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(0, 85, 0, 0);
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.BloodyDawnProj>();
            Item.crit = 14;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Terrarian, 1)
                .AddIngredient(ItemID.Yelets, 1)
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 150)
                .AddIngredient(ItemID.LunarBar, 20)
                .AddIngredient(ModContent.ItemType<BleedingBar>(), 20)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
	}
}
