using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.Common.DropConditions
{
    public class AfterEyeOfCthulhuCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info) => NPC.downedBoss1;

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Drops after Eye of Cthulhu is defeated";
    }
}
