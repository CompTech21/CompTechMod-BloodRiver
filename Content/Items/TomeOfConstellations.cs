using CompTechMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class TomeOfConstellations : ModItem
    {
        internal const float ShootSpeed = 28f;

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 40;
            Item.damage = 36;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 11;
            Item.rare = ItemRarityID.Pink;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.value = Item.buyPrice(gold: 2, silver: 30);
            Item.UseSound = SoundID.Item105;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Constellations>();
            Item.shootSpeed = ShootSpeed;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += 7;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 destination = Main.MouseWorld;
            Vector2 spawnPosition = destination - Vector2.UnitY * 600f; // Высота падения звезды

            int totalProjectiles = 4;
            for (int i = 0; i < totalProjectiles; i++)
            {
                Vector2 offset = new Vector2(MathHelper.Lerp(-160f, 160f, i / (float)(totalProjectiles - 1)), 0);
                Vector2 finalSpawn = spawnPosition + offset + Main.rand.NextVector2Circular(16f, 16f);
                Vector2 newVelocity = (destination - finalSpawn).SafeNormalize(Vector2.UnitY) * ShootSpeed * Main.rand.NextFloat(0.9f, 1.1f);
                Projectile.NewProjectile(source, finalSpawn, newVelocity, type, damage, knockback, player.whoAmI);
            }

            return false;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpellTome)
                .AddIngredient(ItemID.FallenStar, 50)
                .AddIngredient(ItemID.SoulofLight, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
