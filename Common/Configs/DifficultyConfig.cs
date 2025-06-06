using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace CompTechMod.Common.Configs
{
    public class DifficultyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Increase Boss HP by 50%")]
        [DefaultValue(true)]
        public bool IncreaseBossHealth { get; set; } = true;

        [Label("Increase Mob HP by 100%")]
        [DefaultValue(true)]
        public bool IncreaseMobHealth { get; set; } = true;

        [Label("Apply best prefix when reforging weapons")]

        [DefaultValue(false)]
        public bool AlwaysBestWeaponPrefix { get; set; } = true;
    }
}