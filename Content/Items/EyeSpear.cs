using CompTechMod.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class EyeSpear : ModItem
    {
        public override void SetStaticDefaults() {
            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
            ItemID.Sets.Spears[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1);

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 16;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.damage = 18;
            Item.knockBack = 5f;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;

            Item.shootSpeed = 3.7f;
            Item.shoot = ModContent.ProjectileType<EyeSpearProj>();

            Item.scale = 1.2f;
        }

        public override bool CanUseItem(Player player) {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool? UseItem(Player player) {
            if (!Main.dedServ && Item.UseSound.HasValue) {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }
            return null;
        }
    }
}
