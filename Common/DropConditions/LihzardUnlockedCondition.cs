using Terraria.GameContent.ItemDropRules;

namespace CompTechMod.DropConditions
{
	public class LihzardUnlockedCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => Common.Systems.GolemWorldSystem.LihzardUnlocked;

		public bool CanShowItemDropInUI() => true;

		public string GetConditionDescription() => "Drops after Golem is defeated";
	}
}
