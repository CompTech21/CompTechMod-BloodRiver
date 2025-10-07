using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using Terraria.DataStructures;
using StructureHelper.API;

namespace CompTechMod.Common.WorldGeneration
{
    public class TundraDungeonGen : ModSystem
    {
        private static Point16 dungeonCenter = Point16.NegativeOne;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int snowIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (snowIndex != -1)
            {
                tasks.Insert(snowIndex + 1, new PassLegacy("Generate Tundra Dungeon", GenerateTundraDungeon));
            }
        }

        private void GenerateTundraDungeon(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Awakening frozen depths...";

            Point dungeonEntrance = new Point(Main.dungeonX, Main.dungeonY);

            // Поиск границ снежного биома
            int left = -1, right = -1;
            for (int x = 200; x < Main.maxTilesX - 200; x++)
            {
                for (int y = (int)Main.worldSurface; y < Main.rockLayer; y++)
                {
                    if (Main.tile[x, y].HasTile &&
                        (Main.tile[x, y].TileType == TileID.SnowBlock || Main.tile[x, y].TileType == TileID.IceBlock))
                    {
                        if (left == -1)
                            left = x;
                        right = x;
                        break;
                    }
                }
            }

            // Если биом найден
            if (left != -1 && right != -1)
            {
                int centerX = (left + right) / 2;

                // Сканируем глубже, но не слишком низко
                for (int y = (int)(Main.rockLayer + 100); y < Main.maxTilesY - 300; y++)
                {
                    if (Main.tile[centerX, y].HasTile &&
                        (Main.tile[centerX, y].TileType == TileID.IceBlock || Main.tile[centerX, y].TileType == TileID.SnowBlock))
                    {
                        float distToSkeleDungeon = Vector2.Distance(new Vector2(centerX * 16, y * 16),
                            new Vector2(dungeonEntrance.X * 16, dungeonEntrance.Y * 16));

                        if (distToSkeleDungeon < 2000f)
                            continue;

                        // Нашли подходящее место — немного вверх от найденного блока
                        Point16 pos = new Point16(centerX, y - 20);
                        TryPlaceStructure(pos);
                        return;
                    }
                }
            }
        }

        private void TryPlaceStructure(Point16 position)
        {
            string structurePath = "Content/Structures/TundraDungeon.shstruct";
            Generator.GenerateStructure(structurePath, position, CompTechMod.Instance);
            dungeonCenter = position;
        }

        public override void PostUpdatePlayers()
        {
            if (dungeonCenter == Point16.NegativeOne)
                return;

            foreach (Player player in Main.player)
            {
                if (player == null || !player.active)
                    continue;

                float distance = Vector2.Distance(player.Center, dungeonCenter.ToWorldCoordinates());
            }
        }
    }
}
