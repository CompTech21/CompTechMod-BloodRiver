using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CompTechMod.Content.Buffs
{
    public class BleedingBloodDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 60; // Очень сильное кровотечение
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            // Визуальный эффект крови
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.Blood);
                Main.dust[dust].velocity.Y -= 1f;
                Main.dust[dust].scale = 1.2f;
            }
        }
    }
}
