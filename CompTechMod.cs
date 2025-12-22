using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.NPCs;
using CompTechMod.Content.Items;

namespace CompTechMod
{
    public enum CompTechPackets : byte
    {
        SpawnTestDummy,
        KillTestDummies,
        SummonExpiringCore
    }

    public class CompTechMod : Mod
    {
        public static CompTechMod Instance;

        public CompTechMod()
        {
            Instance = this;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            CompTechPackets packetType = (CompTechPackets)reader.ReadByte();

            // ❗ ВСЯ логика — только на сервере
            if (Main.netMode != NetmodeID.Server)
                return;

            switch (packetType)
            {
                // ================= MANEKEN =================
                case CompTechPackets.SpawnTestDummy:
                {
                    Vector2 position = reader.ReadVector2();
                    int playerID = reader.ReadByte();
                    Player player = Main.player[playerID];

                    int index = NPC.NewNPC(
                        player.GetSource_Misc("TestDummy"),
                        (int)position.X,
                        (int)position.Y,
                        ModContent.NPCType<TestDummy>()
                    );

                    if (index >= 0)
                        Main.npc[index].netUpdate = true;

                    break;
                }

                case CompTechPackets.KillTestDummies:
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && npc.type == ModContent.NPCType<TestDummy>())
                        {
                            npc.StrikeInstantKill();
                            npc.netUpdate = true;
                        }
                    }
                    break;
                }

                // ================= BOSS ALTAR =================
                case CompTechPackets.SummonExpiringCore:
                {
                    int playerID = reader.ReadByte();
                    Player player = Main.player[playerID];

                    // ночь?
                    if (Main.dayTime)
                        return;

                    // предмет есть?
                    if (!player.ConsumeItem(ModContent.ItemType<CongealedBlood>()))
                        return;

                    // не даём призвать второго
                    if (NPC.AnyNPCs(ModContent.NPCType<ExpiringCore>()))
                        return;

                    int offsetX = Main.rand.NextBool() ? 400 : -400;
                    Vector2 spawnPos = player.Center + new Vector2(offsetX, 0);

                    int npcIndex = NPC.NewNPC(
                        player.GetSource_Misc("BloodyAltar"),
                        (int)spawnPos.X,
                        (int)spawnPos.Y,
                        ModContent.NPCType<ExpiringCore>()
                    );

                    if (npcIndex >= 0)
                        Main.npc[npcIndex].netUpdate = true;

                    break;
                }
            }
        }
    }
}
