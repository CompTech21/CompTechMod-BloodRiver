using Terraria;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class GlobalDamageMultiplier : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            // Проверка: у предмета должен быть ненулевой урон и тип урона (то есть это оружие)
            if (item.damage > 0 && item.DamageType != DamageClass.Default)
            {
                item.damage = (int)(item.damage * 1.5f);
            }
        }
    }
}
