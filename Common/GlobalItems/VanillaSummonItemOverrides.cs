using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CompTechMod.Common.GlobalItems
{
	public class VanillaSummonItemOverrides : GlobalItem
	{
		public override bool InstancePerEntity => true;

		public override void SetDefaults(Item item)
		{
			if (IsSummonItem(item.type) && !IsException(item.type))
			{
				item.maxStack = 1;
				item.consumable = false;
			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (IsSummonItem(item.type) && !IsException(item.type))
			{
				// Добавляем строку "Не расходуется"
				tooltips.Add(new TooltipLine(Mod, "CompTechMod_NotConsumed", Language.GetTextValue("Mods.CompTechMod.Tooltips.NotConsumed"))
				{
					OverrideColor = new Color(255, 255, 255) // Зелёный цвет, как у баффов
				});
			}
		}

		private bool IsException(int type)
		{
			return type == ItemID.GuideVoodooDoll ||
			       type == ItemID.ClothierVoodooDoll ||
			       type == ItemID.TruffleWorm ||
			       type == ItemID.LihzahrdPowerCell;
		}

		private bool IsSummonItem(int type)
		{
			return type switch
			{
				ItemID.SlimeCrown => true,
				ItemID.SuspiciousLookingEye => true,
				ItemID.WormFood => true,
				ItemID.BloodySpine => true,
				ItemID.Abeemination => true,
				ItemID.DeerThing => true,
				ItemID.QueenSlimeCrystal => true,
				ItemID.MechanicalEye => true,
				ItemID.MechanicalWorm => true,
				ItemID.MechanicalSkull => true,
				ItemID.CelestialSigil => true,
				ItemID.PumpkinMoonMedallion => true,
				ItemID.NaughtyPresent => true,
				ItemID.SnowGlobe => true,
				ItemID.PirateMap => true,
				ItemID.GoblinBattleStandard => true,
				ItemID.BloodMoonStarter => true,
				ItemID.SolarTablet => true,
				_ => false,
			};
		}
	}
}
