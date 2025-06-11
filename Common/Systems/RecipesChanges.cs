using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Items;

namespace CompTechMod.Common.Systems
{
    public class RecipesChanges : ModSystem
    {
        public override void AddRecipes()
        {
            // === ВСЕ ВАНИЛЬНЫЕ ЗЕЛИЯ ===
            int[] allVanillaPotions = new int[]
            {
                ItemID.IronskinPotion,
                ItemID.RegenerationPotion,
                ItemID.SwiftnessPotion,
                ItemID.EndurancePotion,
                ItemID.HeartreachPotion,
                ItemID.HunterPotion,
                ItemID.InvisibilityPotion,
                ItemID.NightOwlPotion,
                ItemID.ObsidianSkinPotion,
                ItemID.ShinePotion,
                ItemID.SpelunkerPotion,
                ItemID.ThornsPotion,
                ItemID.WaterWalkingPotion,
                ItemID.GillsPotion,
                ItemID.FlipperPotion,
                ItemID.BuilderPotion,
                ItemID.FeatherfallPotion,
                ItemID.MiningPotion,
                ItemID.MagicPowerPotion,
                ItemID.ManaRegenerationPotion,
                ItemID.SummoningPotion,
                ItemID.TitanPotion,
                ItemID.BattlePotion,
                ItemID.CalmingPotion,
                ItemID.AmmoReservationPotion,
                ItemID.ArcheryPotion,
                ItemID.LifeforcePotion,
                ItemID.WrathPotion,
                ItemID.RagePotion,
                ItemID.InfernoPotion,
                ItemID.LovePotion,
                ItemID.FishingPotion,
                ItemID.CratePotion,
                ItemID.SonarPotion,
                ItemID.GravitationPotion,
                ItemID.InvisibilityPotion,
                ItemID.TeleportationPotion,
                ItemID.TrapsightPotion,
                ItemID.WarmthPotion,
                ItemID.LuckPotion,
                ItemID.BiomeSightPotion,
            };

            foreach (int potionID in allVanillaPotions)
            {
                Recipe.Create(potionID)
                    .AddIngredient(ItemID.BottledWater, 1)
                    .AddIngredient(ModContent.ItemType<CongealedBlood>(), 10)
                    .AddTile(TileID.Bottles)
                    .Register();
            }

            // --- Твои крафты ниже (оставлены как есть) ---
            Recipe.Create(ItemID.WoodenBoomerang)
                .AddIngredient(ItemID.Wood, 25)
                .AddIngredient(ItemID.Gel, 3)
                .AddTile(TileID.WorkBenches)
                .Register();

            Recipe.Create(ItemID.HermesBoots)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.SwiftnessPotion, 5)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ItemID.CloudinaBottle)
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.Cloud, 15)
                .AddIngredient(ItemID.Feather, 2)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ItemID.RodofDiscord)
                .AddIngredient(ItemID.IceRod)
                .AddIngredient(ItemID.SoulofLight, 20)
                .AddIngredient(ItemID.ChaosElementalBanner)
                .AddTile(TileID.Solidifier)
                .Register();

            Recipe.Create(ItemID.EnchantedSword)
                .AddIngredient(ItemID.GoldBroadsword)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ItemID.EnchantedSword)
                .AddIngredient(ItemID.PlatinumBroadsword)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ItemID.MagicDagger)
                .AddIngredient(ItemID.GoldChest)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            Recipe.Create(ItemID.PhilosophersStone)
                .AddIngredient(ItemID.GoldChest)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            Recipe.Create(ItemID.TitanGlove)
                .AddIngredient(ItemID.GoldChest)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            Recipe.Create(ItemID.StarCloak)
                .AddIngredient(ItemID.GoldChest)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            Recipe.Create(ItemID.CrossNecklace)
                .AddIngredient(ItemID.GoldChest)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            Recipe.Create(ItemID.BandofRegeneration)
                .AddIngredient(ItemID.Shackle)
                .AddIngredient(ItemID.LifeCrystal)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ItemID.AnkletoftheWind)
                .AddIngredient(ItemID.Cloud, 25)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddIngredient(ItemID.PinkGel)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ItemID.Extractinator)
                .AddRecipeGroup("CompTechMod:GoldBar", 10)
                .AddIngredient(ItemID.WaterBucket, 5)
                .AddIngredient(ItemID.Ruby)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
