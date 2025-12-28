using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CompTechMod.Content.Items
{
    public class SuspiciousLookingBait : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Название и описание через локализацию
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Roar;
            Item.rare = ItemRarityID.Purple;
            Item.consumable = false;
            Item.maxStack = 1;
        }

        public override bool CanUseItem(Player player)
        {
            // Запрещаем, если Рыброн уже есть
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.DukeFishron)
                    return false;
            }

            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 offset = new Vector2(
                    Main.rand.NextBool() ? 700 : -700,
                    Main.rand.Next(-200, 200)
                );

                Vector2 spawnPos = player.Center + offset;

                int npcIndex = NPC.NewNPC(
                    player.GetSource_ItemUse(Item),
                    (int)spawnPos.X,
                    (int)spawnPos.Y,
                    NPCID.DukeFishron
                );

                if (npcIndex < Main.maxNPCs)
                    Main.npc[npcIndex].target = player.whoAmI;
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TruffleWorm, 3);
            recipe.AddIngredient(ItemID.Shrimp, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.CookingPots); // котёл
            recipe.Register();
        }
    }
}
