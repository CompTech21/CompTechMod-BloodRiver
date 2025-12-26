using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.NPCs
{
    public class TestDummy : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 36;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 50000000;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.friendly = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.lavaImmune = true;
            NPC.dontCountMe = true;
        }

        public override void AI()
        {
            NPC.lifeRegen = 999;

            NPC.TargetClosest(false);

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
                return;

            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;
    }
}
