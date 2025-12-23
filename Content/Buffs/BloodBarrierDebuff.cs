using Terraria;
using Terraria.ModLoader;

namespace CompTechMod.Content.Buffs
{
    public class BloodBarrierDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
    }
}
