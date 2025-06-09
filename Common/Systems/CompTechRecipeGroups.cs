using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Terraria.GameContent.Creative;

namespace CompTechMod.Common.Systems
{
	public class CompTechRecipeGroups : ModSystem
	{
		public override void AddRecipeGroups() {
			// Дерево
			RecipeGroup groupWood = new RecipeGroup(() => "Any wood", new int[] {
				ItemID.Wood, ItemID.BorealWood, ItemID.RichMahogany, ItemID.Ebonwood,
				ItemID.Shadewood, ItemID.PalmWood, ItemID.Pearlwood
			});
			RecipeGroup.RegisterGroup("CompTechMod:Wood", groupWood);

			// Золотая/Платиновая руда
			RecipeGroup groupGoldBar = new RecipeGroup(() => "Any gold bar", new int[] {
				ItemID.GoldBar, ItemID.PlatinumBar
			});
			RecipeGroup.RegisterGroup("CompTechMod:GoldBar", groupGoldBar);

			// Медный/оловянный слиток
			RecipeGroup groupCopperBar = new RecipeGroup(() => "Any copper bar", new int[] {
				ItemID.CopperBar, ItemID.TinBar
			});
			RecipeGroup.RegisterGroup("CompTechMod:CopperBar", groupCopperBar);

			// Железный/свинцовый слиток
			RecipeGroup groupIronBar = new RecipeGroup(() => "Any iron bar", new int[] {
				ItemID.IronBar, ItemID.LeadBar
			});
			RecipeGroup.RegisterGroup("CompTechMod:IronBar", groupIronBar);

			// Серебряный/вольфрамовый слиток
			RecipeGroup groupSilverBar = new RecipeGroup(() => "Any silver bar", new int[] {
				ItemID.SilverBar, ItemID.TungstenBar
			});
			RecipeGroup.RegisterGroup("CompTechMod:SilverBar", groupSilverBar);

			// Демонит/кримтан
			RecipeGroup groupEvilBar = new RecipeGroup(() => "Any evil bar", new int[] {
				ItemID.DemoniteBar, ItemID.CrimtaneBar
			});
			RecipeGroup.RegisterGroup("CompTechMod:EvilBar", groupEvilBar);

			// Кобальт/палладый
			RecipeGroup groupCobaltBar = new RecipeGroup(() => "Any cobalt bar", new int[] {
				ItemID.CobaltBar, ItemID.PalladiumBar
			});
			RecipeGroup.RegisterGroup("CompTechMod:CobaltBar", groupCobaltBar);

			// Мифрил/орихалк
			RecipeGroup groupMythrilBar = new RecipeGroup(() => "Any mythril bar", new int[] {
				ItemID.MythrilBar, ItemID.OrichalcumBar
			});
			RecipeGroup.RegisterGroup("CompTechMod:MythrilBar", groupMythrilBar);

			// Адамантит/титан
			RecipeGroup groupAdamantiteBar = new RecipeGroup(() => "Any adamantite", new int[] {
				ItemID.AdamantiteBar, ItemID.TitaniumBar
			});
			RecipeGroup.RegisterGroup("CompTechMod:AdamantiteBar", groupAdamantiteBar);

			// Грибы 
			RecipeGroup groupEvilMushrooms = new RecipeGroup(() => "Any evil mushroom", new int[] {
				ItemID.VileMushroom, ItemID.ViciousMushroom
			});
			RecipeGroup.RegisterGroup("CompTechMod:EvilMushrooms", groupEvilMushrooms);

			// ВСЕ КРЫЛЬЯ
			RecipeGroup groupAllWings = new RecipeGroup(() => "Any wings", ItemID.AngelWings); // Ангельские крылья как иконка

			for (int i = 0; i < ItemLoader.ItemCount; i++) {
				Item item = new Item();
				item.SetDefaults(i);

				if (item.accessory && item.wingSlot > 0) {
					groupAllWings.ValidItems.Add(i);
				}
			}
			RecipeGroup.RegisterGroup("CompTechMod:Wings", groupAllWings);
		}
	}
}
