using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CompTechMod.Content.Items
{
    public class TalismanOfAwakening : ModItem
    {
        // Список снимаемых дебаффов
        private static readonly HashSet<int> DebuffsToClear = new HashSet<int>
        {
            BuffID.Bleeding,        // Кровотечение
            BuffID.BrokenArmor,     // Расколотая броня
            BuffID.Stoned,          // Окаменение
            BuffID.Poisoned,        // Отравлен
            BuffID.Silenced,        // Безмолвие
            BuffID.Confused,        // В замешательстве
            BuffID.Slow,            // Медлительность
            BuffID.Cursed,          // Проклят
            BuffID.Weak,            // Слабость
            BuffID.Darkness,        // Мрак
            BuffID.Chilled,         // Переохлаждение
            BuffID.Ichor            // Ихор
        };

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp; // стиль удерживания
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.consumable = false;
            Item.noUseGraphic = false; // предмет всегда виден в руках
            Item.useTurn = true;
            Item.autoReuse = true; // можно удерживать
        }

        public override void HoldItem(Player player)
        {
            // Проверяем, что предмет в активном слоте
            if (player.selectedItem == player.FindItem(Item.type))
            {
                // Проверяем, что игрок зажимает кнопку
                if (player.controlUseItem)
                {
                    // Звук при использовании (раз в цикл)
                    if (player.itemAnimation == player.itemAnimationMax - 1)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item21, player.position);
                    }

                    // -20 к защите
                    player.statDefense -= 20;

                    // +3 к регенерации
                    player.lifeRegen += 5;

                    // Снятие дебаффов
                    for (int i = 0; i < Player.MaxBuffs; i++)
                    {
                        int buffType = player.buffType[i];
                        if (DebuffsToClear.Contains(buffType))
                        {
                            player.DelBuff(i);
                            i--; // корректируем индекс после удаления
                        }
                    }
                }
            }
        }
    }
}
