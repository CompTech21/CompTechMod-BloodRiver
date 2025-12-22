using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Content.Items
{
    public class TestDummySummon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 36;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item44;
            Item.rare = ItemRarityID.White;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            // ===== MULTIPLAYER CLIENT =====
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Mod.GetPacket();

                if (player.altFunctionUse == 2) // ПКМ — удалить
                {
                    packet.Write((byte)CompTechPackets.KillTestDummies);
                }
                else // ЛКМ — заспавнить
                {
                    packet.Write((byte)CompTechPackets.SpawnTestDummy);
                    packet.WriteVector2(Main.MouseWorld);
                    packet.Write((byte)player.whoAmI);
                }

                packet.Send();
                return true;
            }

            // ===== SINGLEPLAYER (сервер = клиент) =====
            if (player.altFunctionUse != 2)
            {
                NPC.NewNPC(
                    player.GetSource_ItemUse(Item),
                    (int)Main.MouseWorld.X,
                    (int)Main.MouseWorld.Y,
                    ModContent.NPCType<Content.NPCs.TestDummy>()
                );
            }
            else
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.type == ModContent.NPCType<Content.NPCs.TestDummy>())
                        npc.StrikeInstantKill();
                }
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TargetDummy)
                .Register();
        }
    }
}
