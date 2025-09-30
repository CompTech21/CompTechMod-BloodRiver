using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using CompTechMod.Content.Items;

namespace CompTechMod.Common.Systems
{
    public class CompWorld : ModSystem
    {
        public static bool DontDoThisMode = false;

        // Сохраняем в заголовок мира (отображается в меню)
        public override void SaveWorldHeader(TagCompound tag)
        {
            if (DontDoThisMode)
                tag["dontDoThisMode"] = true;
        }

        // Загружаем при входе в мир
        public override void LoadWorldData(TagCompound tag)
        {
            DontDoThisMode = tag.ContainsKey("dontDoThisMode") && tag.GetBool("dontDoThisMode");
        }

        // Сохраняем при выходе из мира
        public override void SaveWorldData(TagCompound tag)
        {
            if (DontDoThisMode)
                tag["dontDoThisMode"] = true;
        }

        public override void OnWorldUnload()
        {
            DontDoThisMode = false;
        }
    }
}
