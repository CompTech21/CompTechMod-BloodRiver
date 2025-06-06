using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CompTechMod.Common.Systems
{
    public class BleedingOreSystem : ModSystem
    {
        private bool _oreGenerated;

        public override void OnWorldLoad()
        {
            _oreGenerated = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["BleedingOreGenerated"] = _oreGenerated;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            _oreGenerated = tag.ContainsKey("BleedingOreGenerated") && tag.GetBool("BleedingOreGenerated");
        }

        public override void PostUpdateWorld()
        {
            if (!_oreGenerated && NPC.downedMoonlord)
            {
                GenerateOreVeins();
                _oreGenerated = true;

                Color darkRed = new Color(139, 0, 0);
                Main.NewText("The world is blessed, and in its depths pulses the Bleeding Ore.", darkRed);
            }
        }

        private void GenerateOreVeins()
        {
            int totalOreTarget = 5000;
            int averageVeinSize = 10;
            int veinCount = totalOreTarget / (averageVeinSize * 5);

            for (int i = 0; i < veinCount; i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.66), Main.maxTilesY - 200);

                WorldGen.TileRunner(x, y, averageVeinSize, WorldGen.genRand.Next(5, 10), (ushort)ModContent.TileType<Content.Tiles.BleedingOre>());

                ReplaceNearbyTilesWithOre(x, y, averageVeinSize + 4);
            }

            Terraria.WorldGen.SquareTileFrame(0, 0, true);
        }

        private void ReplaceNearbyTilesWithOre(int centerX, int centerY, int radius)
        {
            int minX = Math.Max(centerX - radius, 0);
            int maxX = Math.Min(centerX + radius, Main.maxTilesX - 1);
            int minY = Math.Max(centerY - radius, 0);
            int maxY = Math.Min(centerY + radius, Main.maxTilesY - 1);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (Vector2.Distance(new Vector2(centerX, centerY), new Vector2(x, y)) <= radius)
                    {
                        Tile tile = Main.tile[x, y];
                        if (tile != null && tile.HasTile)
                        {
                            if (tile.TileType == TileID.Stone
                                || tile.TileType == TileID.Dirt
                                || tile.TileType == TileID.Mud
                                || tile.TileType == TileID.ClayBlock
                                || tile.TileType == TileID.Slush)
                            {
                                tile.TileType = (ushort)ModContent.TileType<Content.Tiles.BleedingOre>();
                            }
                        }
                    }
                }
            }
        }
    }
}
