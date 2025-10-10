using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CompTechMod.Content.Items
{
    public class RainbowJewel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Название и описание задаются через локализацию
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Pink;
            Item.consumable = false;
            Item.maxStack = 1;
        }

        public override bool? UseItem(Player player)
        {
            // Проверка: есть ли уже Императрица Света
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.HallowBoss)
                    return false;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Смещение для спавна босса относительно игрока
                Vector2 offset = new Vector2(Main.rand.NextBool() ? 600 : -600, Main.rand.Next(-200, 200));
                Vector2 spawnPos = player.Center + offset;

                int npcIndex = NPC.NewNPC(null, (int)spawnPos.X, (int)spawnPos.Y, NPCID.HallowBoss);
                if (npcIndex < Main.maxNPCs)
                    Main.npc[npcIndex].target = player.whoAmI;
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ItemID.PixieDust, 25);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ItemID.UnicornHorn, 5);
            recipe.AddIngredient(ItemID.EmpressButterfly, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
