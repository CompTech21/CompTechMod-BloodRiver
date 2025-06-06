using CompTechMod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CompTechMod.Content
{
    public class StarterItems : ModPlayer
    {
        private bool itemsGiven = false;

        public override void OnEnterWorld()
        {
            if (!itemsGiven)
            {
                Player.QuickSpawnItem(null, ModContent.ItemType<StarterBag>(), 1);

                itemsGiven = true;
            }
        }

        // Сохраняем флаг в персонажа
        public override void SaveData(TagCompound tag)
        {
            tag["itemsGiven"] = itemsGiven;
        }

        // Загружаем флаг из персонажа
        public override void LoadData(TagCompound tag)
        {
            itemsGiven = tag.GetBool("itemsGiven");
        }
    }
}



//for github