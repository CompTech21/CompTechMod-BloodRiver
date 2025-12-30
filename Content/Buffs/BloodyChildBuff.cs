using Terraria;
using Terraria.ModLoader;

namespace CompTechMod.Content.Buffs
{
    public class BloodyChildBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 🔥 ВАЖНЕЙШАЯ СТРОКА
            player.buffTime[buffIndex] = 18000;

            if (player.ownedProjectileCounts[
                ModContent.ProjectileType<Projectiles.BloodyChildMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
