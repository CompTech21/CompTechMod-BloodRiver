using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class RecipesChanges : ModSystem
    {
        public override void AddRecipes()
        {
            // Деревянный бумеранг
            Recipe.Create(ItemID.WoodenBoomerang)
                .AddIngredient(ItemID.Wood, 25)
                .AddIngredient(ItemID.Gel, 3)
                .AddTile(TileID.WorkBenches)
                .Register();

            // Сапоги гермеса
            Recipe.Create(ItemID.HermesBoots, 1)
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.SwiftnessPotion, 5)
                .AddTile(TileID.Anvils)
                .Register();
            
            // Облако в бутылке
            Recipe.Create(ItemID.CloudinaBottle, 1)
                .AddIngredient(ItemID.Bottle, 1)
                .AddIngredient(ItemID.Cloud, 15)
                .AddIngredient(ItemID.Feather, 2)
                .AddTile(TileID.Anvils)
                .Register();
            
            // Жезл раздора
            Recipe.Create(ItemID.RodofDiscord, 1)
                .AddIngredient(ItemID.IceRod, 1)
                .AddIngredient(ItemID.SoulofLight, 20)
                .AddIngredient(ItemID.ChaosElementalBanner, 1)
                .AddTile(TileID.Solidifier)
                .Register();
            
            // Зачарованный меч (gold bar)
            Recipe.Create(ItemID.EnchantedSword, 1)
                .AddIngredient(ItemID.GoldBroadsword, 1)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddTile(TileID.Anvils)
                .Register();
            
            // Зачарованный меч (platinum bar)
            Recipe.Create(ItemID.EnchantedSword, 1)
                .AddIngredient(ItemID.PlatinumBroadsword, 1)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddTile(TileID.Anvils)
                .Register();
            
            // Магический кинжал
            Recipe.Create(ItemID.MagicDagger, 1)
                .AddIngredient(ItemID.GoldChest, 1)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            
            // Философский камень
            Recipe.Create(ItemID.PhilosophersStone, 1)
                .AddIngredient(ItemID.GoldChest, 1)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            
            // Титановая перчатка
            Recipe.Create(ItemID.TitanGlove, 1)
                .AddIngredient(ItemID.GoldChest, 1)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            
            // Звездный плащ
            Recipe.Create(ItemID.StarCloak, 1)
                .AddIngredient(ItemID.GoldChest, 1)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            
            // Крестик
            Recipe.Create(ItemID.CrossNecklace, 1)
                .AddIngredient(ItemID.GoldChest, 1)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            
            // Кольцо регенерации
            Recipe.Create(ItemID.BandofRegeneration, 1)
                .AddIngredient(ItemID.Shackle, 1)
                .AddIngredient(ItemID.LifeCrystal, 1)
                .AddTile(TileID.Anvils)
                .Register();
            
            // Браслет ветра
            Recipe.Create(ItemID.AnkletoftheWind, 1)
                .AddIngredient(ItemID.Cloud, 25)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddIngredient(ItemID.PinkGel, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}