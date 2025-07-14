using CompTechMod.Content.Buffs;
using CompTechMod.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using CompTechMod.DropConditions;

namespace CompTechMod.Content.NPCs
{
    public class BleedingSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4; // Указываем количество кадров анимации
        }

        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 56; // 224 / 4 = 56
            NPC.damage = 145;
            NPC.defense = 30;
            NPC.lifeMax = 4500;
            NPC.knockBackResist = 0.4f;
            NPC.value = Item.buyPrice(silver: 9, copper: 40);
            NPC.aiStyle = NPCAIStyleID.Slime;
            AIType = NPCID.BlueSlime; // Поведение обычного слизня
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < (NPC.life <= 0 ? 30 : 3); i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.RedTorch, hit.HitDirection, -1f);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && NPC.downedMoonlord && spawnInfo.Player.ZoneRockLayerHeight && !spawnInfo.PlayerSafe)
            {
                return 0.15f; // 15% шанс в подземельях после Мунлорда
            }
            return 0f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
            {
                target.AddBuff(ModContent.BuffType<BleedingBloodDebuff>(), 300); // 5 секунд
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BleedingOreItem>(), chanceDenominator: 1, minimumDropped: 10, maximumDropped: 26));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry entry)
        {
            entry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement(" ")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.2f;
            if (NPC.frameCounter >= Main.npcFrameCount[NPC.type])
                NPC.frameCounter = 0;

            NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
        }
    }
}
