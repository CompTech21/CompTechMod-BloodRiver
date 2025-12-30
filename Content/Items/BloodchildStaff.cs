using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CompTechMod.Content.Items
{
    public class BloodchildStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 460;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 16;
            Item.width = 60;
            Item.height = 62;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item44;

            Item.shoot = ModContent.ProjectileType<Projectiles.BloodyChildMinion>();
            Item.buffType = ModContent.BuffType<Buffs.BloodyChildBuff>();
        }

        public override bool Shoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            player.AddBuff(Item.buffType, 18000);

            // 📍 СПАВН В ТОЧКЕ КУРСОРА
            Vector2 spawnPos = Main.MouseWorld;

            Projectile.NewProjectile(
                source,
                spawnPos,
                Vector2.Zero,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StardustCellStaff, 1)
                .AddIngredient(ItemID.StardustDragonStaff, 1)
                .AddIngredient(ModContent.ItemType<CongealedBlood>(), 150)
                .AddIngredient(ItemID.LunarBar, 20)
                .AddIngredient(ModContent.ItemType<BleedingBar>(), 20)
                .AddIngredient(ModContent.ItemType<BloodEssence>(), 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
