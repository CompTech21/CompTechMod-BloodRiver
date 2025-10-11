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
    public class ForestHouseGen : ModSystem
    {
        private static Point16 houseCenter = Point16.NegativeOne;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int surfaceIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (surfaceIndex != -1)
            {
                tasks.Insert(surfaceIndex + 1, new PassLegacy("Generate Forest House", GenerateForestHouse));
            }
        }

        private void GenerateForestHouse(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Building cozy forest home...";

            int spawnX = (int)(Main.maxTilesX / 2f);
            int searchRange = 100; // Радиус поиска от спавна (~100 тайлов)

            // Ищем подходящее место рядом со спавном
            for (int x = spawnX - searchRange; x < spawnX + searchRange; x += 5)
            {
                // Пропускаем, если это не лес (например, пустыня, снег и т.п.)
                if (IsBadBiome(x))
                    continue;

                int surfaceY = FindSurfaceY(x);
                if (surfaceY <= 0)
                    continue;

                // Проверим, что дом не будет в воде
                if (IsWaterHere(x, surfaceY))
                    continue;

                // Ставим структуру прямо на поверхность
                Point16 position = new Point16(x, surfaceY - 10);
                TryPlaceStructure(position);
                return;
            }
        }

        private void TryPlaceStructure(Point16 position)
        {
            string structurePath = "Content/Structures/ForestHouse.shstruct";

            // Приклеиваем дом к земле
            Point16 fixedPos = SnapToSurface(position);

            Generator.GenerateStructure(structurePath, fixedPos, CompTechMod.Instance);
            houseCenter = fixedPos;
        }

        private static Point16 SnapToSurface(Point16 pos)
        {
            // Ищем ближайшую твердую землю под позицией
            for (int y = pos.Y; y < Main.maxTilesY - 100; y++)
            {
                if (Main.tile[pos.X, y].HasTile && Main.tileSolid[Main.tile[pos.X, y].TileType])
                {
                    // Ставим прямо НА землю, а не под неё
                    return new Point16(pos.X, y - 10);
                }
            }
            return pos;
        }

        private static int FindSurfaceY(int x)
        {
            // Новый алгоритм: ищем последний воздух над землёй
            for (int y = 0; y < Main.worldSurface; y++)
            {
                if (Main.tile[x, y].HasTile && Main.tileSolid[Main.tile[x, y].TileType])
                {
                    // Когда нашли землю — возвращаем предыдущий тайл (где воздух)
                    return y - 1;
                }
            }
            return -1;
        }

        private static bool IsWaterHere(int x, int y)
        {
            for (int i = -2; i <= 2; i++)
            {
                if (Main.tile[x + i, y].LiquidAmount > 50)
                    return true;
            }
            return false;
        }

        private static bool IsBadBiome(int x)
        {
            // Проверяем тип блоков на поверхности
            for (int y = (int)(Main.worldSurface - 50); y < Main.worldSurface; y++)
            {
                if (!Main.tile[x, y].HasTile)
                    continue;

                ushort tileType = Main.tile[x, y].TileType;
                if (tileType == TileID.Sand || tileType == TileID.SnowBlock || tileType == TileID.IceBlock || tileType == TileID.Mud)
                    return true;
            }
            return false;
        }

        public override void PostUpdatePlayers()
        {
            if (houseCenter == Point16.NegativeOne)
                return;

            // Пример — можно добавить особые эффекты рядом с домом
        }
    }
}
