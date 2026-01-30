using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Common.Players;
using CompTechMod.Common.DamageClasses;
using CompTechMod.Content.Projectiles;

namespace CompTechMod.Content.Items
{
    public class SparkGun : ModItem
    {
        private int step;

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = ModContent.GetInstance<PyrotechnicDamageClass>();
            Item.width = 48;
            Item.height = 26;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shootSpeed = 8f;
            Item.shoot = ModContent.ProjectileType<SparkGunProj>();
            Item.UseSound = SoundID.Item12;
            Item.knockBack = 0.5f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<PyrotechnicPlayer>().CanUsePyroWeapons;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var pyro = player.GetModPlayer<PyrotechnicPlayer>();
            pyro.AddHeat(true);

            bool isStar = step == 1;

            var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);

            if (isStar && proj.ModProjectile is SparkGunProj sparkProj)
                sparkProj.IsStar = true;

            step = (step + 1) % 3;
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (!player.channel)
                player.GetModPlayer<PyrotechnicPlayer>().ResetEffects();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FlareGun);
            recipe.AddIngredient(ItemID.CopperBar, 5);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
