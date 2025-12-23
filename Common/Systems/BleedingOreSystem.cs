using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.Chat;

namespace CompTechMod.Common.Systems
{
    public class BleedingOreSystem : ModSystem
    {
        public static bool BleedingOreUnlocked { get; private set; }
        private static bool messagePrinted;

        public override void OnWorldLoad()
        {
            BleedingOreUnlocked = false;
            messagePrinted = false;
        }

        public override void OnWorldUnload()
        {
            BleedingOreUnlocked = false;
            messagePrinted = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["BleedingOreUnlocked"] = BleedingOreUnlocked;
            tag["BleedingOreMessagePrinted"] = messagePrinted;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            BleedingOreUnlocked = tag.ContainsKey("BleedingOreUnlocked") && tag.GetBool("BleedingOreUnlocked");
            messagePrinted = tag.ContainsKey("BleedingOreMessagePrinted") && tag.GetBool("BleedingOreMessagePrinted");
        }

        public override void PostUpdateNPCs()
        {
            // Проверка: убит Moonlord и ещё не открыта руда
            if (!BleedingOreUnlocked && NPC.downedMoonlord)
            {
                BleedingOreUnlocked = true;
                GenerateOreVeins();
                PrintMessage();
            }
        }

        private void PrintMessage()
        {
            if (messagePrinted) return;
            messagePrinted = true;

            Color darkRed = new Color(255, 0, 0);
            string text = Language.GetTextValue("Mods.CompTechMod.Messages.BleedingOreBlessed");

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), darkRed);
            }
            else
            {
                Main.NewText(text, darkRed);
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
                                || tile.TileType == TileID.Sand
                                || tile.TileType == TileID.Sandstone
                                || tile.TileType == TileID.Crimsand
                                || tile.TileType == TileID.Crimstone
                                || tile.TileType == TileID.HallowSandstone
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
