using Terraria;
using Terraria.ModLoader;

namespace CompTechMod.Content.Buffs
{
    public class EctoplasmDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
    }
}



//DisplayName.SetDefault("Эктоплазменное Ослабление");
//Description.SetDefault("Ваша защита нарушена. Эффект Призрачного Завета неактивен.");