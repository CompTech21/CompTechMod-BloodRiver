using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;

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

            // ================= EXPIRING CORE =================

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                "ExpiringCore",
                19.0f,
                (Func<bool>)(() => CompTechModSystem.downedExpiringCore),
                ModContent.NPCType<Content.NPCs.ExpiringCore>(),
                new Dictionary<string, object>
                {
                    ["spawnItems"] = ModContent.ItemType<Content.Items.BloodyAltarItem>(),
                    ["collectibles"] = new List<int>(),
                    ["spawnInfo"] = "Призывается при использовании застывшей крови на кровавом алтаре ночью",
                    ["customBossInfo"] = "Входит в ярость за пределами багрянца"
                }
            );

            // ================= ДОБАВЛЯЕМ ПРИЗЫВАЛКИ К ВАНИЛЬНЫМ БОССАМ =================

            bossChecklistMod.Call(
                "SubmitEntrySpawnItems",
                Mod,
                new Dictionary<string, object>
                {
                    // Plantera
                    { "Terraria Plantera", ModContent.ItemType<Content.Items.ArtificialBulb>() },

                    // Empress of Light
                    { "Terraria HallowBoss", ModContent.ItemType<Content.Items.RainbowJewel>() },

                    // Duke Fishron (РЫБРОН)
                    { "Terraria DukeFishron", ModContent.ItemType<Content.Items.SuspiciousLookingBait>() }
                }
            );
        }
    }
}
