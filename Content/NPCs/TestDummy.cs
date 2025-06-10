using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;

namespace CompTechMod.Content.NPCs
{
    public class TestDummy : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 36;
            NPC.aiStyle = -1; // Без AI
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 500000000;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamageFromHostiles = true; // Не получает урон от врагов
            NPC.friendly = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.dontCountMe = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false; // Столкновение с блоками
            NPC.lavaImmune = true; // Иммунитет к лаве
        }

        public override void AI()
        {
            NPC.lifeRegen = 999; // Регенерация HP (по 2 за тик)

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(true);
                return;
            }

            if (player.Center.X > NPC.Center.X)
                NPC.direction = 1; // Игрок справа — смотрим вправо
            else
                NPC.direction = -1; // Игрок слева — смотрим влево

            NPC.spriteDirection = NPC.direction;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A special object with a huge amount of health. Perfect for evaluating the effectiveness of your equipment.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f; // Не спавнится сам
    }
}
