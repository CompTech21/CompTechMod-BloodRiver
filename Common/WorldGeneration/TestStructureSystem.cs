using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;

namespace CompTechMod.Common.WorldGeneration
{
    public class TestStructureSystem : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int spawnIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Spawn Point"));

            if (spawnIndex != -1)
            {
                tasks.Insert(
                    spawnIndex + 1,
                    new PassLegacy(
                        "Guide House",
                        (progress, _) => GenerateGuideHouse(progress)
                    )
                );
            }
        }

        private void GenerateGuideHouse(GenerationProgress progress)
        {
            progress.Message = "Generating guide house";

            int baseX = Main.spawnTileX + 14;

            int houseWidth = 11;
            int houseHeight = 8;

            int groundY = FindFlatGround(baseX, Main.spawnTileY, houseWidth);
            if (groundY == -1)
                return;

            int startX = baseX;
            int startY = groundY - houseHeight + 1;

            ClearArea(startX, startY, houseWidth, houseHeight);
            BuildHouse(startX, startY, houseWidth, houseHeight);
        }

        private static int FindFlatGround(int startX, int startY, int width)
        {
            for (int y = startY - 10; y < startY + 25; y++)
            {
                bool flat = true;

                for (int x = startX; x < startX + width; x++)
                {
                    if (!WorldGen.InWorld(x, y))
                        return -1;

                    Tile tile = Main.tile[x, y];
                    if (!tile.HasTile || !Main.tileSolid[tile.TileType])
                    {
                        flat = false;
                        break;
                    }
                }

                if (flat)
                    return y;
            }

            return -1;
        }

        private static void ClearArea(int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    if (!WorldGen.InWorld(x, y))
                        continue;

                    WorldGen.KillTile(x, y, false, false, true);
                    WorldGen.KillWall(x, y, false);
                }
            }
        }

        private static void BuildHouse(int startX, int startY, int width, int height)
        {
            // коробка + стены
            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    bool border =
                        x == startX ||
                        x == startX + width - 1 ||
                        y == startY ||
                        y == startY + height - 1;

                    if (border)
                    {
                        WorldGen.PlaceTile(x, y, TileID.WoodBlock, true, true);
                    }
                    else
                    {
                        WorldGen.PlaceWall(x, y, WallID.Wood);
                    }
                }
            }

            // === ДВЕРНОЙ ПРОЁМ ===
            int doorX = startX + width / 2;
            int doorBottomY = startY + height - 2;

            for (int y = doorBottomY - 2; y <= doorBottomY; y++)
            {
                WorldGen.KillTile(doorX, y);
                WorldGen.KillWall(doorX, y);
            }

            WorldGen.PlaceObject(doorX, doorBottomY, TileID.ClosedDoor);

            // === СТУЛ ===
            WorldGen.PlaceObject(startX + 2, startY + height - 2, TileID.Chairs);

            // === СУНДУК ===
            int chestX = startX + width - 3;
            int chestY = startY + height - 2;

            WorldGen.KillTile(chestX, chestY);

            int chestIndex = WorldGen.PlaceChest(
                chestX,
                chestY,
                (ushort)TileID.Containers,
                false,
                0
            );

            if (chestIndex >= 0)
            {
                Chest chest = Main.chest[chestIndex];
                chest.item[0].SetDefaults(ItemID.Torch);
                chest.item[0].stack = 3;
            }

            // === КАРТИНА ===
            WorldGen.PlaceObject(
                startX + width / 2 - 1,
                startY + 2,
                TileID.Painting3X2
            );
        }
    }
}
