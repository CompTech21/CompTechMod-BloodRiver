using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CompTechMod.Content.Items;
using Microsoft.Xna.Framework;

namespace CompTechMod.Common.Systems
{
    public class DontDoThisEffects : ModSystem
    {
        public override void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate)
        {
            if (!CompWorld.DontDoThisMode)
                return;

            if (!Main.dayTime)
            {
                timeRate *= 0.5; // ночь ×2 медленнее
            }
        }

        public override void PostUpdateWorld()
        {
            if (!CompWorld.DontDoThisMode)
                return;

            // Всегда Blood Moon ночью
            if (!Main.dayTime)
                Main.bloodMoon = true;
        }

        public override void PostUpdatePlayers()
        {
            if (!CompWorld.DontDoThisMode)
                return;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                // Все характеристики уменьшены (кроме HP и маны)
                player.statDefense = (int)(player.statDefense * 0.5f);
                player.GetDamage(DamageClass.Generic) *= 0.5f;
                player.moveSpeed *= 0.5f;
                player.pickSpeed *= 4f;

                // -1 слот аксессуаров
                if (player.extraAccessorySlots > 0)
                    player.extraAccessorySlots--;

                // 5% шанс спавна 8 Dungeon Guardian
                if (Main.rand.NextFloat() < 0.05f)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 spawnPos = player.Center + new Vector2(Main.rand.Next(-400, 400), Main.rand.Next(-400, 400));
                        int npcIndex = NPC.NewNPC(null, (int)spawnPos.X, (int)spawnPos.Y, NPCID.DungeonGuardian);
                        if (npcIndex >= 0 && Main.npc[npcIndex] != null)
                        {
                            Main.npc[npcIndex].target = player.whoAmI;
                            Main.npc[npcIndex].friendly = false;
                            Main.npc[npcIndex].npcSlots = 10f;
                        }
                    }
                }
            }
        }
    }

    // Удаление оружия/инструмента
    public class DontDoThisItemGlobal : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if (!CompWorld.DontDoThisMode)
                return true;

            if (item.DamageType != DamageClass.Default || item.pick > 0 || item.axe > 0 || item.hammer > 0)
            {
                if (Main.rand.NextFloat() < 0.02f)
                {
                    item.TurnToAir();
                    return false;
                }
            }
            return true;
        }
    }

    // Замена крафта
    public class DontDoThisRecipe : GlobalItem
    {
        public override void OnCraft(Item item, Recipe recipe)
        {
            if (!CompWorld.DontDoThisMode)
                return;

            if (Main.rand.NextFloat() < 0.15f)
            {
                item.SetDefaults(ItemID.Poo);
            }
        }
    }
}
