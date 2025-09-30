using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class OceanRing : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 2, 30, 0);
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // +1 регенерации
            player.lifeRegen += 1;

            // +1 к защите
            player.statDefense += 1;

            // +5 максимального здоровья
            player.statLifeMax2 += 5;

            // Проверка на воду
            if (player.wet)
            {
                // Под водой — если стоит на месте и не атакует
                if (player.velocity.Length() == 0f && !player.controlUseItem && !player.controlUseTile)
                {
                    if (player.breath < player.breathMax)
                        player.breath += 2;
                }

                // Свет под водой
                player.AddBuff(BuffID.Shine, 2);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BandofRegeneration, 1);
            recipe.AddIngredient(ModContent.ItemType<SeaSplinter>(), 30);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}