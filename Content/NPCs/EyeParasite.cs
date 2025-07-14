using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.Content.NPCs
{
    public class EyeParasite : ModNPC
    {
        private int shootTimer;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 64;
            NPC.damage = 60;
            NPC.defense = 12;
            NPC.lifeMax = 120;
            NPC.knockBackResist = 0.5f;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.value = Item.buyPrice(silver: 3);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            // Поведение как у Creeper от Мозга Ктулху
            NPC.aiStyle = 2;
            AIType = NPCID.DemonEye;
        }

        public override void AI()
        {
            shootTimer++;
            if (shootTimer >= 300) // Каждые 5 секунд
            {
                shootTimer = 0;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 velocity = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 10f;

                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        velocity,
                        ProjectileID.BloodShot,
                        40,
                        1f,
                        Main.myPlayer
                    );
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
            {
                target.AddBuff(BuffID.Weak, 240);
                target.AddBuff(BuffID.Bleeding, 240);
            }
        }

                public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime && // Только ночью
                !spawnInfo.PlayerSafe &&
                spawnInfo.Player.ZoneCrimson &&
                (spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneDirtLayerHeight))
            {
                return 0.10f;
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry entry)
        {
            entry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement(" ")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

        }
    }
}
