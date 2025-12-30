using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CompTechMod.Content.NPCs;
using Terraria.Localization;
using Terraria.Chat;

namespace CompTechMod.Common.Systems
{
    public class BossRushSystem : ModSystem
    {
        public static bool Active;
        public static int Stage;
        public static int SpawnDelay;

        private static Vector2[] returnPositions;

        public struct BossEntry
        {
            public int Type;
            public bool Night;
            public bool TeleportToHell;

            public BossEntry(int type, bool night = false, bool hell = false)
            {
                Type = type;
                Night = night;
                TeleportToHell = hell;
            }
        }

        public static readonly List<BossEntry> Bosses = new()
        {
            new(NPCID.KingSlime),
            new(NPCID.EyeofCthulhu, night:true),
            new(NPCID.QueenBee),
            new(NPCID.SkeletronHead, night:true),
            new(NPCID.Deerclops),
            new(NPCID.WallofFlesh, hell:true),
            new(NPCID.QueenSlimeBoss),
            new(NPCID.TheDestroyer, night:true),
            new(NPCID.Spazmatism, night:true),
            new(NPCID.Retinazer, night:true),
            new(NPCID.SkeletronPrime, night:true),
            new(NPCID.Plantera),
            new(NPCID.Golem),
            new(NPCID.HallowBoss, night:true),
            new(NPCID.DukeFishron),
            new(NPCID.CultistBoss),
            new(NPCID.MoonLordCore),
            new(ModContent.NPCType<ExpiringCore>())
        };

        public static void Start(Player starter)
        {
            if (Active)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText("Boss Rush is already active!", Color.OrangeRed);
                return;
            }

            Active = true;
            Stage = 0;
            SpawnDelay = 120;

            returnPositions = new Vector2[Main.maxPlayers];
            for (int i = 0; i < Main.maxPlayers; i++)
                if (Main.player[i].active)
                    returnPositions[i] = Main.player[i].Center;

            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText("Boss Rush has begun!", Color.DarkRed);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Boss Rush has begun!"), Color.DarkRed);
        }

        public static void OnBossKilled(NPC npc)
        {
            if (!Active) return;

            // EoW
            if (Bosses[Stage].Type == NPCID.EaterofWorldsHead)
            {
                if (!NPC.AnyNPCs(NPCID.EaterofWorldsHead) &&
                    !NPC.AnyNPCs(NPCID.EaterofWorldsBody) &&
                    !NPC.AnyNPCs(NPCID.EaterofWorldsTail))
                {
                    Stage++;
                }
            }
            else if (npc.type == Bosses[Stage].Type)
            {
                Stage++;
            }

            // Телепорт после WoF
            if (npc.type == NPCID.WallofFlesh)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active)
                    {
                        Main.player[i].Teleport(returnPositions[i]);

                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, i, returnPositions[i].X, returnPositions[i].Y, 1);
                    }
                }
            }

            if (Stage >= Bosses.Count)
                End();
        }

        private static void SpawnBoss(BossEntry boss)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Player p = Main.player[Player.FindClosest(Vector2.Zero, 1, 1)];

            if (boss.Night)
            {
                Main.dayTime = false;
                Main.time = 0;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData);
            }

            if (boss.TeleportToHell)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (!Main.player[i].active) continue;
                    returnPositions[i] = Main.player[i].Center;
                    Vector2 hellCenter = new Vector2(Main.maxTilesX / 2 * 16, Main.UnderworldLayer * 16);
                    Main.player[i].Teleport(hellCenter);

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, i, hellCenter.X, hellCenter.Y, 1);
                }

                NPC.SpawnWOF(p.position);
                return;
            }

            // Близнецы
            if (boss.Type == NPCID.Spazmatism)
            {
                if (!NPC.AnyNPCs(NPCID.Spazmatism))
                {
                    int spaz = NPC.NewNPC(new EntitySource_WorldEvent(), (int)p.Center.X, (int)p.Center.Y - 300, NPCID.Spazmatism);
                    if (Main.netMode == NetmodeID.Server && spaz >= 0)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spaz);
                }
                if (!NPC.AnyNPCs(NPCID.Retinazer))
                {
                    int ret = NPC.NewNPC(new EntitySource_WorldEvent(), (int)p.Center.X, (int)p.Center.Y - 300, NPCID.Retinazer);
                    if (Main.netMode == NetmodeID.Server && ret >= 0)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, ret);
                }
                return;
            }

            // Golem, Cultist, MoonLord, Duke Fishron через NewNPC
            if (boss.Type == NPCID.Golem || boss.Type == NPCID.CultistBoss || boss.Type == NPCID.MoonLordCore || boss.Type == NPCID.DukeFishron)
            {
                if (!NPC.AnyNPCs(boss.Type))
                {
                    int x = (int)p.Center.X;
                    int y = (int)p.Center.Y;

                    if (boss.Type == NPCID.DukeFishron)
                        y -= 300;
                    else if (boss.Type == NPCID.Golem)
                        y -= 600;
                    else if (boss.Type == NPCID.CultistBoss || boss.Type == NPCID.MoonLordCore)
                        y -= 400;

                    int npcID = NPC.NewNPC(new EntitySource_WorldEvent(), x, y, boss.Type);

                    if (Main.netMode == NetmodeID.Server && npcID >= 0 && npcID < Main.maxNPCs)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcID);
                }
                return;
            }

            // Остальные боссы через NewNPC для правильного скейлинга HP
            if (!NPC.AnyNPCs(boss.Type))
            {
                int x = (int)p.Center.X;
                int y = (int)p.Center.Y - 300;

                int npcID = NPC.NewNPC(new EntitySource_WorldEvent(), x, y, boss.Type);

                if (Main.netMode == NetmodeID.Server && npcID >= 0 && npcID < Main.maxNPCs)
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcID);
            }
        }

        private static bool AreAllPlayersDead()
        {
            bool anyAlive = false;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && !Main.player[i].ghost)
                {
                    anyAlive = true;
                    break;
                }
            }
            return !anyAlive;
        }

        public override void PostUpdateWorld()
        {
            if (!Active) return;

            // Проверка на смерть всех игроков
            if (AreAllPlayersDead())
            {
                End(failed: true);
                return;
            }

            if (Stage >= Bosses.Count) { End(); return; }

            bool bossAlive = false;

            if (Bosses[Stage].Type == NPCID.Spazmatism || Bosses[Stage].Type == NPCID.Retinazer)
                bossAlive = NPC.AnyNPCs(NPCID.Spazmatism) || NPC.AnyNPCs(NPCID.Retinazer);
            else if (Bosses[Stage].Type == NPCID.EaterofWorldsHead)
                bossAlive = NPC.AnyNPCs(NPCID.EaterofWorldsHead) ||
                            NPC.AnyNPCs(NPCID.EaterofWorldsBody) ||
                            NPC.AnyNPCs(NPCID.EaterofWorldsTail);
            else
                bossAlive = NPC.AnyNPCs(Bosses[Stage].Type);

            if (bossAlive) return;

            SpawnDelay--;
            if (SpawnDelay > 0) return;

            SpawnDelay = 180;
            SpawnBoss(Bosses[Stage]);
        }

        private static void End(bool failed = false)
        {
            Active = false;
            Stage = 0;

            // Удаляем всех боссов при провале
            if (failed)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n.active && n.boss)
                    {
                        n.active = false;
                        n.netUpdate = true;
                    }
                }
            }

            string message = failed ? "Boss Rush failed!" : "Boss Rush completed!";
            Color color = failed ? Color.Red : Color.Gold;

            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(message, color);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
        }
    }
}