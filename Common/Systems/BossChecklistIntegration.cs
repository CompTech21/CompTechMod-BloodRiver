using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace CompTechMod.Common.Systems
{
    public class BossChecklistIntegration : ModSystem
    {
        public override void PostSetupContent()
        {
            IntegrateWithBossChecklist();
        }

        private void IntegrateWithBossChecklist()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
                return;

            if (bossChecklistMod.Version < new Version(1, 6))
                return;

            // Регистрируем ExpiringCore
            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                "ExpiringCore",
                19.0f,
                (Func<bool>)(() => CompTechModSystem.downedExpiringCore),
                ModContent.NPCType<Content.NPCs.ExpiringCore>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<Content.Items.BloodyAltarItem>(),
                    ["collectibles"] = new List<int>()
                    {

                    },
                    ["spawnInfo"] = "Призывается при использовании застывшей крови на кровавом алтаре ночью",
                    ["customBossInfo"] = "Входит в ярость за пределами багрянца"
                }
            );

            // Добавляем кастомную призывалку для Plantera (правильный способ!)
            bossChecklistMod.Call(
                "SubmitEntrySpawnItems",
                Mod,
                new Dictionary<string, object>()
                {
                    { "Terraria Plantera", ModContent.ItemType<Content.Items.ArtificialBulb>() },
                    { "Terraria HallowBoss", ModContent.ItemType<Content.Items.RainbowJewel>() }
                }
            );
        }
    }
}