using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CompTechMod.Common.Configs;

namespace CompTechMod.Common.Global
{
    public class BestReforge : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.damage > 0 && !entity.accessory; // Только оружие, не аксессуары
        }

        public override void PostReforge(Item item)
        {
            // Проверка конфигурации: если отключено — ничего не делать
            if (!ModContent.GetInstance<DifficultyConfig>().AlwaysBestWeaponPrefix)
                return;

            if (item.damage <= 0 || item.accessory)
                return;

            int bestPrefix = GetBestPrefix(item);

            if (bestPrefix > 0)
            {
                item.Prefix(bestPrefix);
                // Визуально обновим характеристики предмета
            }
        }

        private int GetBestPrefix(Item item)
        {
            if (item.CountsAsClass(DamageClass.Melee))
                return PrefixID.Legendary;

            if (item.CountsAsClass(DamageClass.Ranged))
                return PrefixID.Unreal;

            if (item.CountsAsClass(DamageClass.Magic))
                return PrefixID.Mythical;

            if (item.CountsAsClass(DamageClass.Summon))
                return PrefixID.Ruthless;

            if (item.CountsAsClass(DamageClass.Throwing)) // На случай кастомного оружия
                return PrefixID.Godly;

            return 0; // Тип не определён — ничего не делать
        }
    }
}