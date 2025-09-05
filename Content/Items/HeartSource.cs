using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Linq;

namespace CompTechMod.Content.Items
{
    public class HeartSource : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = -12;
            Item.value = Item.sellPrice(platinum: 15);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 3f;
            player.GetCritChance(DamageClass.Generic) += 100f;
            player.GetAttackSpeed(DamageClass.Melee) += 1.2f;
            player.GetAttackSpeed(DamageClass.Ranged) += 1.2f;
            player.GetAttackSpeed(DamageClass.Magic) += 1.2f;

            player.moveSpeed += 1.5f;
            player.maxRunSpeed += 1.5f;

            player.wingTimeMax = (int)(player.wingTimeMax * 3f);

            player.statDefense += 100;
            player.statLifeMax2 += 100;
            player.statManaMax2 += 500;
            player.endurance += 0.70f;
            player.GetArmorPenetration(DamageClass.Generic) += 100;

            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[BuffID.Blackout] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.WitheredArmor] = true;
            player.buffImmune[BuffID.WitheredWeapon] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Ichor] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Webbed] = true;
            player.buffImmune[BuffID.Stoned] = true;
            player.buffImmune[BuffID.Obstructed] = true;
            player.buffImmune[BuffID.Electrified] = true;
            player.buffImmune[BuffID.ManaSickness] = true;
            player.buffImmune[BuffID.ChaosState] = true;
            player.buffImmune[BuffID.ShadowFlame] = true;
            player.noKnockback = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UnityEmblem>());
            recipe.AddIngredient(ModContent.ItemType<AnnihilationEmblem>());
            recipe.AddIngredient(ItemID.AnkhShield);
            recipe.AddIngredient(ItemID.LunarBar, 40);
            recipe.AddIngredient(ModContent.ItemType<BleedingBar>(), 40);
            recipe.AddIngredient(ItemID.LifeFruit, 20);
            recipe.AddIngredient(ItemID.ManaCrystal, 25);
            recipe.AddIngredient(ModContent.ItemType<CongealedBlood>(), 500);
            recipe.AddIngredient(ItemID.FragmentSolar, 35);
            recipe.AddIngredient(ItemID.FragmentVortex, 35);
            recipe.AddIngredient(ItemID.FragmentNebula, 35);
            recipe.AddIngredient(ItemID.FragmentStardust, 35);
            recipe.AddIngredient(ModContent.ItemType<BloodEssence>(), 35);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
