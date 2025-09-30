using Terraria;
using Terraria.ModLoader;
using CompTechMod.Content.Items;

namespace CompTechMod.Common.Systems
{
    public class DontDoThisEffects : ModSystem
    {
        // Правильный способ замедлить ночь
        public override void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate)
        {
            if (!CompWorld.DontDoThisMode)
                return;

            if (!Main.dayTime)
            {
                timeRate *= 0.5; // ночь ×2
            }
        }

        public override void PostUpdateWorld()
        {
            if (!CompWorld.DontDoThisMode)
                return;

            // 2. Всегда Blood Moon ночью
            if (!Main.dayTime)
                Main.bloodMoon = true;
        }

        public override void PostUpdatePlayers()
        {
            if (!CompWorld.DontDoThisMode)
                return;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                // 3. Все характеристики уменьшаем в 2 раза (кроме HP и маны)
                player.statDefense *= 0.5f; // работает с DefenseStat
                player.GetDamage(DamageClass.Generic) *= 0.5f;
                player.moveSpeed *= 0.5f;
                player.pickSpeed *= 2f; // копает хуже

                // 4. -1 слот аксессуаров
                if (player.extraAccessorySlots > 0)
                    player.extraAccessorySlots = player.extraAccessorySlots - 1;
            }
        }
    }
}
