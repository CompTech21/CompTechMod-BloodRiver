using Terraria;
using Terraria.ModLoader;
using Terraria.IO;
using Terraria.ModLoader.IO;

namespace CompTechMod.Common.Systems
{
    public class CompTechModSystem : ModSystem
    {
        public static bool downedExpiringCore = false;

        public override void OnWorldLoad()
        {
            downedExpiringCore = false;
        }

        public override void OnWorldUnload()
        {
            downedExpiringCore = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (downedExpiringCore)
                tag["downedExpiringCore"] = true;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedExpiringCore = tag.ContainsKey("downedExpiringCore") && tag.GetBool("downedExpiringCore");
        }
    }
}