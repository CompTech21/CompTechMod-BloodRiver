// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using Terraria;
// using Terraria.ID;
// using Terraria.IO;
// using Terraria.ModLoader;
// using Terraria.WorldBuilding;
// using Terraria.GameContent.Generation;
// using Terraria.DataStructures;
// using StructureHelper.API;

// namespace CompTechMod.Common.WorldGeneration
// {
//     public class ForestHouseGen : ModSystem
//     {
//         private static Point16 houseCenter = Point16.NegativeOne;

//         public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
//         {
//             int index = tasks.FindIndex(pass => pass.Name == "Micro Biomes");
//             if (index != -1)
//             {
//                 tasks.Insert(index + 1, new PassLegacy("Generate Underground House", GenerateUndergroundHouse));
//             }
//         }

//         private void GenerateUndergroundHouse(GenerationProgress progress, GameConfiguration configuration)
//         {
//             progress.Message = "Generating underground structure...";

//             int spawnX = Main.spawnTileX;  // ТОЧНОЕ место спавна по X
//             int startY = (int)Main.rockLayer;     // каменный слой
//             int endY = (int)(Main.maxTilesY * 0.75);

//             int requiredStoneCount = 120; // сколько каменных тайлов подряд должно быть (регулируется)
//             int currentStone = 0;

//             for (int y = startY; y < endY; y++)
//             {
//                 Tile tile = Main.tile[spawnX, y];
//                 if (tile.HasTile && IsStoneLike(tile.TileType))
//                 {
//                     currentStone++;
//                 }
//                 else
//                 {
//                     currentStone = 0;
//                 }

//                 // как только накопилось много камня — генерируем
//                 if (currentStone >= requiredStoneCount)
//                 {
//                     int placeY = y - 30; // небольшое поднятие над слоем камня
//                     TryPlaceStructure(new Point16(spawnX - 25, placeY)); // центрируем структуру
//                     return;
//                 }
//             }
//         }

//         private bool IsStoneLike(ushort type)
//         {
//             return type == TileID.Stone ||
//                    type == TileID.Dirt ||
//                    type == TileID.ClayBlock ||
//                    type == TileID.Marble ||
//                    type == TileID.Granite ||
//                    type == TileID.SnowBlock ||
//                    Main.tileSolid[type];
//         }

//         private void TryPlaceStructure(Point16 pos)
//         {
//             string path = "Content/Structures/ForestHouse.shstruct";

//             Generator.GenerateStructure(path, pos, CompTechMod.Instance);
//             houseCenter = pos;
//         }
//     }
// }
