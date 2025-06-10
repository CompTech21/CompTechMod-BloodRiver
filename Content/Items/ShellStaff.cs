using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Projectiles;

namespace CompTechMod.Content.Items
{
    public class ShellStaff : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 42;
            Item.height = 42;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ShellStaffProj>();
            Item.shootSpeed = 11f;
            Item.staff[this.Type] = true;
            Item.crit = 10;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Выстреливает множество снарядов
            for (int i = 0; i < 1; i++) // 1 снаряд за выстрел
            {
                Vector2 modifiedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10)); // Немного случайного отклонения для снарядов
                Projectile.NewProjectile(source, position, modifiedVelocity, type, damage, knockback, player.whoAmI);
            }
            return false; // Чтобы не создавался снаряд по умолчанию
        }
    }
}
