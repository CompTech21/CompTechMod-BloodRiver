using Terraria.ModLoader;
using Terraria.GameInput;

namespace CompTechMod.Common.Keybinds
{
    public class CompTechKeybinds : ModSystem
    {
        public static ModKeybind BloodCarapaceKey;

        public override void Load()
        {
            BloodCarapaceKey = KeybindLoader.RegisterKeybind(
                Mod,
                "Blood Carapace Ability",
                "Q"
            );
        }

        public override void Unload()
        {
            BloodCarapaceKey = null;
        }
    }
}
