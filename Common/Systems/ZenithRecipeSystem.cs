using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CompTechMod.Common.Systems
{
    public class ZenithRecipeSystem : ModSystem
    {
        public override void PostAddRecipes()
        {
            foreach (Recipe recipe in Main.recipe)
            {
                if (recipe.createItem.type == ItemID.Zenith)
                {
                    recipe.AddIngredient(ModContent.ItemType<Content.Items.BloodEssence>(), 25);
                }
            }
        }
    }
}
