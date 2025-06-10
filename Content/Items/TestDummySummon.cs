using CompTechMod.Content.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item44;
            Item.noUseGraphic = false;
            Item.autoReuse = false;
            Item.consumable = false;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2) // ПКМ
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.type == ModContent.NPCType<TestDummy>())
                    {
                        npc.active = false;
                    }
                }
            }
            else // ЛКМ
            {
                Vector2 spawnPosition = Main.MouseWorld;
                int index = NPC.NewNPC(
                    player.GetSource_ItemUse(Item),
                    (int)spawnPosition.X, (int)spawnPosition.Y,
                    ModContent.NPCType<TestDummy>());

                if (index < Main.maxNPCs)
                {
                    Main.npc[index].netUpdate = true;
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
