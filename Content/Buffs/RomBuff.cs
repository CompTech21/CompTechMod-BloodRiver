using Terraria;
using Terraria.ModLoader;

namespace CompTechMod.Content.Buffs
{
    public class RomBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.20f;     // +20% урона
            player.moveSpeed += 0.15f;                           // +15% скорость
            player.statDefense -= 5;                             // -5 защиты
        }
    }
}