using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CompTechMod.Content.Items
{
    public class BloodShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Название и описание через локализацию
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;

            Item.rare = ItemRarityID.LightPurple;
            Item.value = 0;

            Item.consumable = false;
            Item.maxStack = 1;
        }

        public override bool CanUseItem(Player player)
        {
            // Только во время кровавой луны
            if (!Main.bloodMoon)
                return false;

            // Если дреднаутилус уже жив — нельзя
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.BloodNautilus)
                    return false;
            }

            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 spawnPos = player.Center + new Vector2(
                    Main.rand.NextBool() ? 700 : -700,
                    Main.rand.Next(-200, 200)
                );

                int npc = NPC.NewNPC(
                    player.GetSource_ItemUse(Item),
                    (int)spawnPos.X,
                    (int)spawnPos.Y,
                    NPCID.BloodNautilus
                );

                if (npc < Main.maxNPCs)
                    Main.npc[npc].target = player.whoAmI;
            }

            return true;
        }
    }
}
