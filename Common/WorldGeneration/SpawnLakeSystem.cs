using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;

namespace CompTechMod.Common.WorldGeneration
{
    public class SpawnLakeSystem : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int spawnIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Spawn Point"));

            if (spawnIndex != -1)
            {
                tasks.Insert(
                    spawnIndex + 1,
                    new PassLegacy(
                        "Spawn Lake",
                        (progress, _) => GenerateSpawnLake(progress)
                    )
                );
            }
        }

        private void GenerateSpawnLake(GenerationProgress progress)
        {
            progress.Message = "Generating spawn lake";

            int centerX = Main.spawnTileX + 40;

            int lakeWidth = 60;
            int lakeDepth = 18;

            int groundY = FindFlatGround(centerX - lakeWidth / 2, Main.spawnTileY, lakeWidth);
            if (groundY == -1)
                return;

            DigLake(centerX, groundY, lakeWidth, lakeDepth);
            FillWithWater(centerX, groundY, lakeWidth, lakeDepth);
        }

        private static int FindFlatGround(int startX, int startY, int width)
        {
            for (int y = startY - 15; y < startY + 40; y++)
            {
                bool valid = true;

                for (int x = startX; x < startX + width; x++)
                {
                    if (!WorldGen.InWorld(x, y))
                        return -1;

                    Tile tile = Main.tile[x, y];
                    if (!tile.HasTile || !Main.tileSolid[tile.TileType])
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                    return y;
            }

            return -1;
        }

        private static void DigLake(int centerX, int surfaceY, int width, int depth)
        {
            int halfWidth = width / 2;

            for (int x = -halfWidth; x <= halfWidth; x++)
            {
                float shape = (float)Math.Sin((x + halfWidth) / (float)width * Math.PI);
                int localDepth = (int)(shape * depth);

                for (int y = 0; y <= localDepth; y++)
                {
                    int worldX = centerX + x;
                    int worldY = surfaceY + y;

                    if (!WorldGen.InWorld(worldX, worldY))
                        continue;

                    WorldGen.KillTile(worldX, worldY, false, false, true);
                    WorldGen.KillWall(worldX, worldY, false);
                }
            }
        }
        private static void FillWithWater(int centerX, int surfaceY, int width, int depth)
        {
            int halfWidth = width / 2;

            for (int x = -halfWidth; x <= halfWidth; x++)
            {
                for (int y = 1; y <= depth; y++)
                {
                    int worldX = centerX + x;
                    int worldY = surfaceY + y;

                    if (!WorldGen.InWorld(worldX, worldY))
                        continue;

                    Tile tile = Main.tile[worldX, worldY];
                    tile.LiquidType = LiquidID.Water;
                    tile.LiquidAmount = 255;
                }
            }
        }
    }
}
