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
    public class DesertDungeonGen : ModSystem
    {
        // Сохраняем позицию сгенерированной структуры
        private static Point16 dungeonCenter = Point16.NegativeOne;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int desertIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (desertIndex != -1)
            {
                tasks.Insert(desertIndex + 1, new PassLegacy("Generate Desert Dungeon", GenerateDesertDungeon));
            }
        }

        private void GenerateDesertDungeon(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Summoning ancient sands...";

            // Поиск подземной пустыни
            for (int i = 100; i < Main.maxTilesX - 100; i += 50)
            {
                for (int j = (int)Main.rockLayer; j < Main.maxTilesY - 200; j++)
                {
                    if (Main.tile[i, j].TileType == TileID.Sandstone)
                    {
                        Point16 position = new Point16(i, j);
                        TryPlaceStructure(position);
                        return;
                    }
                }
            }
        }

        private void TryPlaceStructure(Point16 position)
        {
            string structurePath = "Content/Structures/DesertDungeon.shstruct";

            Generator.GenerateStructure(structurePath, position, CompTechMod.Instance);

            // Запоминаем центр структуры
            dungeonCenter = position;
        }

        public override void PostUpdatePlayers()
        {
            if (dungeonCenter == Point16.NegativeOne) 
                return; // структура не найдена или не сгенерирована

            foreach (Player player in Main.player)
            {
                if (player == null || !player.active)
                    continue;

                // Проверяем расстояние до центра структуры
                float distance = Vector2.Distance(player.Center, dungeonCenter.ToWorldCoordinates());

                // Если игрок очень близко (200 пикселей ≈ 12.5 тайлов)
                if (distance < 200f)
                {
                    player.AddBuff(BuffID.NoBuilding, 10); // накладываем дебафф NoBuilding (обновляется каждый тик)
                }
            }
        }
    }
}
