using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CompTechMod.Common.Systems;

namespace CompTechMod.Content.NPCs
{
    public class DeepSeaShark : ModNPC
    {
        private enum SharkState
        {
            Idle,
            Aggro
        }

        private SharkState currentState = SharkState.Idle;
        private int attackTimer = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;

            // Показывать на анализаторе форм жизни
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 250;
            NPC.height = 250;
            NPC.damage = 40;
            NPC.defense = 10;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.knockBackResist = 0.2f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 3);
            NPC.lavaImmune = true;
            NPC.npcSlots = 10f;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return NPC.downedBoss1 && spawnInfo.Player.ZoneBeach && spawnInfo.Water ? 0.15f : 0f;
        }

        public override void AI()
        {
            Player target = Main.player[NPC.target];

            // Плавный поворот к игроку (рот слева)
            Vector2 directionToPlayer = target.Center - NPC.Center;
            float angleToPlayer = directionToPlayer.ToRotation() + MathHelper.Pi;
            float smoothRotation = 0.1f;
            NPC.rotation = NPC.rotation.AngleTowards(angleToPlayer, smoothRotation);

            switch (currentState)
            {
                case SharkState.Idle:
                    NPC.velocity *= 0.95f;

                    if (NPC.justHit)
                    {
                        currentState = SharkState.Aggro;
                        NPC.TargetClosest();
                    }
                    break;

                case SharkState.Aggro:
                    attackTimer++;

                    Vector2 dashDirection = target.Center - NPC.Center;
                    dashDirection.Normalize();

                    if (attackTimer % 180 == 0)
                    {
                        NPC.velocity = dashDirection * 12f;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int shark = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.Shark);
                            Main.npc[shark].velocity = dashDirection * 4f;
                        }
                    }

                    NPC.velocity *= 0.96f;
                    break;
            }
        }

        public override void OnKill()
        {
            // Лут выпадает на землю
            var source = NPC.GetSource_Loot();

            CompTechModSystem.downedDeepSeaShark = true;

            Item.NewItem(source, NPC.getRect(), ItemID.Seashell, Main.rand.Next(10, 16));
            Item.NewItem(source, NPC.getRect(), ItemID.Starfish, Main.rand.Next(10, 16));
            Item.NewItem(source, NPC.getRect(), ItemID.SharkFin, Main.rand.Next(4, 8));

            if (Main.rand.NextFloat() < 0.25f)
                Item.NewItem(source, NPC.getRect(), ItemID.SharkToothNecklace);

            Item.NewItem(source, NPC.getRect(), ItemID.GoldCoin, 3);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 0;
        }
    }
}
