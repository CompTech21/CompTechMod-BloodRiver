using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Chat;
using Terraria.Localization;
using CompTechMod.Content.Items;

namespace CompTechMod.Content.Tiles
{
    public class BloodyAltar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.addTile(Type);

            DustType = DustID.Blood;
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

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)CompTechPackets.SummonExpiringCore);
                packet.Write(player.whoAmI);
                packet.Send();
                return true;
            }

            TrySummon(player);
            return true;
        }

        public static void TrySummon(Player player)
        {
            if (Main.dayTime)
            {
                ChatHelper.BroadcastChatMessage(
                    NetworkText.FromLiteral("At night..."),
                    Color.DarkRed
                );
                return;
            }

            if (!player.ConsumeItem(ModContent.ItemType<CongealedBlood>()))
            {
                ChatHelper.BroadcastChatMessage(
                    NetworkText.FromLiteral("Congealed Blood is needed!"),
                    Color.Red
                );
                return;
            }

            if (NPC.AnyNPCs(ModContent.NPCType<Content.NPCs.ExpiringCore>()))
                return;

            int offsetX = Main.rand.NextBool() ? 400 : -400;
            Vector2 spawnPos = player.Center + new Vector2(offsetX, 0);

            NPC.NewNPC(
                player.GetSource_Misc("BloodyAltar"),
                (int)spawnPos.X,
                (int)spawnPos.Y,
                ModContent.NPCType<Content.NPCs.ExpiringCore>()
            );

            ChatHelper.BroadcastChatMessage(
                NetworkText.FromLiteral("The core begins to awaken..."),
                Color.DarkRed
            );
        }
    }
}
