using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace CompTechMod.Content.Items
{
    public class BloodyTears : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.damage = 320;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 17;
            Item.useTime = 7;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(gold: 70);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.BloodyTearsProj>();
            Item.shootSpeed = 11f;
            Item.crit = 23;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberOfProjectiles = 5;
            float speed = velocity.Length();
            Vector2 realPlayerPos;

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                realPlayerPos = new Vector2(
                    player.position.X + player.width * 0.5f + Main.rand.Next(-300, 300),
                    player.MountedCenter.Y - 600f + Main.rand.Next(-50, 50)
                );

                Vector2 target = Main.MouseWorld;
                Vector2 direction = target - realPlayerPos;
                if (direction.Y < 20f) direction.Y = 20f;
                direction.Normalize();
                direction *= speed;

                direction.Y += Main.rand.NextFloat(-0.8f, 0.8f);

                Projectile.NewProjectile(source, realPlayerPos, direction,
                    ModContent.ProjectileType<Projectiles.BloodyTearsProj>(),
                    damage, knockback, player.whoAmI);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteorStaff, 1)
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 150)
                .AddIngredient(ItemID.LunarBar, 20)
                .AddIngredient(ModContent.ItemType<BleedingBar>(), 20)
                .AddIngredient(ModContent.ItemType<BloodEssence>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
