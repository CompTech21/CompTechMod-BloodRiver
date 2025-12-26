using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using CompTechMod.Content.NPCs;
using Terraria.Localization;

namespace CompTechMod.Common.Systems
{
    public class BossIntroSystem : ModSystem
    {
        private static bool active;
        private static int timer;
        private static BossIntro currentIntro;

        private const int ShowTime = 180;
        private const int FadeTime = 30;

        // Кто сейчас отображается
        private static int activeBossWhoAmI = -1;

        public override void OnWorldLoad()
        {
            active = false;
            timer = 0;
            activeBossWhoAmI = -1;
        }

        public override void OnWorldUnload()
        {
            active = false;
            timer = 0;
            activeBossWhoAmI = -1;
        }

        public override void PostUpdateNPCs()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            // Обновляем таймер текущего интро
            if (active)
            {
                timer++;
                if (timer >= ShowTime)
                {
                    active = false;
                    // НЕ сбрасываем activeBossWhoAmI здесь, оставляем до смерти босса
                }
            }

            // Проверяем активных боссов
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || !npc.boss)
                    continue;

                // Если интро уже для этого босса активно, пропускаем
                if (npc.whoAmI == activeBossWhoAmI)
                    break;

                // Если интро не активно для текущего босса
                if (BossData.TryGetValue(npc.type, out BossIntro intro))
                {
                    StartIntro(intro, npc.whoAmI);
                    break;
                }
            }

            // Сбрасываем activeBossWhoAmI, если босс умер
            if (activeBossWhoAmI != -1)
            {
                NPC boss = Main.npc[activeBossWhoAmI];
                if (!boss.active || !boss.boss)
                    activeBossWhoAmI = -1;
            }
        }

        private void StartIntro(BossIntro intro, int bossWhoAmI)
        {
            currentIntro = intro;
            timer = 0;
            active = true;
            activeBossWhoAmI = bossWhoAmI;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(l => l.Name == "Vanilla: Mouse Text");
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "CompTechMod: Boss Intro",
                    DrawIntro,
                    InterfaceScaleType.UI
                ));
            }
        }

        private bool DrawIntro()
        {
            if (!active)
                return true;

            float alpha = 1f;
            if (timer < FadeTime)
                alpha = timer / (float)FadeTime;
            else if (timer > ShowTime - FadeTime)
                alpha = (ShowTime - timer) / (float)FadeTime;

            alpha = MathHelper.Clamp(alpha, 0f, 1f);

            Vector2 center = new Vector2(Main.screenWidth / 2f, 120f);

            Color nameColor = currentIntro.Color * alpha;
            Color descColor = currentIntro.Color * 0.8f * alpha;

            Utils.DrawBorderStringBig(Main.spriteBatch, currentIntro.Name, center, nameColor, 0.8f, 0.5f, 0.5f);
            Utils.DrawBorderString(Main.spriteBatch, currentIntro.Description, center + new Vector2(0, 60), descColor, 1f, 0.5f, 0.5f);

            return true;
        }

        private static readonly Dictionary<int, BossIntro> BossData = new()
        {
            { NPCID.KingSlime, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.KingSlime.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.KingSlime.Description"),
                new Color(60, 130, 250)) },

            { NPCID.EyeofCthulhu, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.EyeofCthulhu.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.EyeofCthulhu.Description"),
                new Color(230, 230, 230)) },

            { NPCID.EaterofWorldsHead, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.EaterOfWorlds.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.EaterOfWorlds.Description"),
                new Color(150, 110, 200)) },

            { NPCID.BrainofCthulhu, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.BrainOfCthulhu.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.BrainOfCthulhu.Description"),
                new Color(240, 140, 140)) },

            { NPCID.QueenBee, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.QueenBee.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.QueenBee.Description"),
                new Color(255, 200, 0)) },

            { NPCID.SkeletronHead, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.Skeletron.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.Skeletron.Description"),
                new Color(170, 160, 145)) },

            { NPCID.Deerclops, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.Deerclops.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.Deerclops.Description"),
                new Color(160, 210, 255)) },

            { NPCID.WallofFlesh, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.WallOfFlesh.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.WallOfFlesh.Description"),
                new Color(180, 60, 60)) },

            { NPCID.QueenSlimeBoss, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.QueenSlime.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.QueenSlime.Description"),
                new Color(255, 160, 255)) },

            { NPCID.TheDestroyer, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.TheDestroyer.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.TheDestroyer.Description"),
                new Color(210, 50, 50)) },

            { NPCID.Retinazer, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.TheTwins.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.TheTwins.Description"),
                new Color(255, 100, 80)) },

            { NPCID.Spazmatism, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.TheTwins.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.TheTwins.Description"),
                new Color(255, 100, 80)) },

            { NPCID.SkeletronPrime, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.SkeletronPrime.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.SkeletronPrime.Description"),
                new Color(190, 190, 190)) },

            { NPCID.Plantera, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.Plantera.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.Plantera.Description"),
                new Color(100, 220, 100)) },

            { NPCID.Golem, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.Golem.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.Golem.Description"),
                new Color(255, 140, 40)) },

            { NPCID.DukeFishron, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.DukeFishron.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.DukeFishron.Description"),
                new Color(80, 250, 220)) },

            { NPCID.HallowBoss, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.EmpressOfLight.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.EmpressOfLight.Description"),
                new Color(255, 210, 255)) },

            { NPCID.CultistBoss, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.LunaticCultist.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.LunaticCultist.Description"),
                new Color(120, 140, 255)) },

            { NPCID.MoonLordCore, new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.MoonLord.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.MoonLord.Description"),
                new Color(150, 255, 200)) },

            { ModContent.NPCType<ExpiringCore>(), new BossIntro(
                Language.GetTextValue("Mods.CompTechMod.NPCs.ExpiringCore.DisplayName"),
                Language.GetTextValue("Mods.CompTechMod.NPCs.ExpiringCore.Description"),
                new Color(255, 110, 20)) }
                };

    }

    public struct BossIntro
    {
        public string Name;
        public string Description;
        public Color Color;

        public BossIntro(string name, string description, Color color)
        {
            Name = name;
            Description = description;
            Color = color;
        }
    }
}
