using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

#nullable disable
namespace CompTechMod.Content.Items
{
    public class VanillaValueEdit : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.WormholePotion)
                item.value = Item.buyPrice(0, 1, 15, 0); // 1 золотых 15 серебрянных

            if (item.type == ItemID.BottledWater)
                item.value = Item.buyPrice(0, 0, 7, 0); // 7 серебра

            if (item.type == ItemID.JungleRose)
                item.value = Item.buyPrice(0, 5, 15, 0); // 5 золотых 15 серебрянных

            if (item.type == ItemID.GoldenKey)
                item.value = Item.buyPrice(0, 7, 0, 0); // 7 золотых

            if (item.type == ItemID.RodofDiscord)
                item.value = Item.buyPrice(0, 50, 0, 0); // 50 золотых
        }
    }
}
