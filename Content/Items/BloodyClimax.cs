using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class BloodyClimax : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 525;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 126;
            Item.height = 32;
            Item.useTime = 20; // 1/3 секунда
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.crit = 41;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Стандартный выстрел
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            // Кровавый эффект
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(position, 1, 1, DustID.Blood, velocity.X * 0.2f, velocity.Y * 0.2f);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
            }

            return false; // Чтобы не стрелял двойной пулей
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SniperRifle, 1)
                .AddIngredient(ItemID.FragmentVortex, 25)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ModContent.ItemType<BleedingBar>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
