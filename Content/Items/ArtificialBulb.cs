using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CompTechMod.Content.Items
{
    public class ArtificialBulb : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Название и описание — через локализацию
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
            Item.maxStack = 1;
        }

        public override bool? UseItem(Player player)
        {
            if (!player.ZoneJungle)
                return false;

            // Проверка: есть ли уже Плантера в мире
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.Plantera)
                    return false;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Смещение от игрока (~40 блоков)
                Vector2 offset = new Vector2(Main.rand.NextBool() ? 640 : -640, Main.rand.Next(-200, 200));
                Vector2 spawnPos = player.Center + offset;

                int npcIndex = NPC.NewNPC(null, (int)spawnPos.X, (int)spawnPos.Y, NPCID.Plantera);
                if (npcIndex < Main.maxNPCs)
                    Main.npc[npcIndex].target = player.whoAmI;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Color purple = new Color(150, 0, 255);
                Main.NewText("Плантера пробудилась", purple);
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.JungleSpores, 10);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
