using Terraria.ModLoader;

namespace CompTechMod.Common.DamageClasses
{
    public class PyrotechnicDamageClass : DamageClass
    {
        public override bool UseStandardCritCalcs => true;

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }
    }
}
