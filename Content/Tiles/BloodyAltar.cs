using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
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

            // ===== MULTIPLAYER CLIENT =====
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)CompTechPackets.SummonExpiringCore);
                packet.Write((byte)player.whoAmI);
                packet.Send();
                return true;
            }

            // ===== SINGLEPLAYER =====
            if (Main.dayTime)
            {
                Main.NewText("At night...", Color.DarkRed);
                return true;
            }

            if (!player.ConsumeItem(ModContent.ItemType<CongealedBlood>()))
            {
                Main.NewText("Congealed Blood is needed!", Color.Red);
                return true;
            }

            if (NPC.AnyNPCs(ModContent.NPCType<Content.NPCs.ExpiringCore>()))
                return true;

            int offsetX = Main.rand.NextBool() ? 400 : -400;
            Vector2 spawnPos = player.Center + new Vector2(offsetX, 0);

            NPC.NewNPC(
                player.GetSource_Misc("BloodyAltar"),
                (int)spawnPos.X,
                (int)spawnPos.Y,
                ModContent.NPCType<Content.NPCs.ExpiringCore>()
            );

            return true;
        }
    }
}
