using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.Common.Systems
{
    public class GlobalBossBagDrops : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            // 1. Королева слизней — эссенции света
            if (item.type == ItemID.QueenSlimeBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(ItemID.SoulofLight, 1, 7, 11));
            }

            // 2. Мозг Ктулху — эссенции ночи (только в хардмоде)
            if (item.type == ItemID.BrainOfCthulhuBossBag)
            {
                itemLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ItemID.SoulofNight, 1, 7, 11));
            }

            // 2. Пожиратель миров — эссенции ночи (только в хардмоде)
            if (item.type == ItemID.EaterOfWorldsBossBag)
            {
                itemLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ItemID.SoulofNight, 1, 7, 11));
            }
        }
    }
}