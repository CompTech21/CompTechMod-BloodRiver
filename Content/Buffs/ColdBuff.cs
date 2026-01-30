using Terraria;
using Terraria.ModLoader;

namespace CompTechMod.Content.Buffs
{
    public class ColdBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}
