using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using CompTechMod.Content.NPCs;
using CompTechMod.Content.Items;

namespace CompTechMod.Content.Tiles
{
    public class BloodyAltar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            // Размер под твою текстуру 50x32: делаем 3x2 "клетки" (≈48x32)
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 3; 
            TileObjectData.newTile.Height = 2; 
            TileObjectData.newTile.CoordinateWidth = 16; 
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.addTile(Type);

            DustType = DustID.Blood;
            AnimationFrameHeight = 32;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<CongealedBlood>();
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            // Можно вызвать только ночью
            if (Main.dayTime)
            {
                Main.NewText("At night...", Color.DarkRed);
                return true;
            }

            // Пытаемся потребить одну "Congealed Blood"
            if (!player.ConsumeItem(ModContent.ItemType<CongealedBlood>()))
            {
                Main.NewText("Congealed Blood is needed!", Color.Red);
                return true;
            }

            // Спавним босса НЕ на алтаре, а рядом с игроком (справа или слева)
            // Это делаем только на сервере/в одиночке
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Выбор стороны и расстояния
                int horizontalOffset = Main.rand.NextBool() ? 400 : -400; // примерно 25 tiles
                int spawnX = (int)(player.Center.X + horizontalOffset);
                int spawnY = (int)player.Center.Y;

                // Подготовим временный Item, чтобы корректно передать источник события
                Item tmp = new Item();
                tmp.SetDefaults(ModContent.ItemType<CongealedBlood>());

                // Создаём NPC с корректным источником
                int npcIndex = NPC.NewNPC(player.GetSource_ItemUse(tmp), spawnX, spawnY, ModContent.NPCType<ExpiringCore>());

                // Если мы на сервере — синхронизируем созданный NPC всем клиентам
                if (Main.netMode == NetmodeID.Server && npcIndex >= 0)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: npcIndex);
                }

            }

            return true;
        }
    }
}
