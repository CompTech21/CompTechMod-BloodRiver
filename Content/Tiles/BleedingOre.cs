using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace CompTechMod.Content.Tiles
{
    public class BleedingOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileShine[Type] = 1100;
            Main.tileShine2[Type] = true;
            Main.tileOreFinderPriority[Type] = 950;

            TileID.Sets.Ore[Type] = true;

            AddMapEntry(new Color(190, 30, 40), Language.GetText("Mods.CompTechMod.Tiles.BleedingOre.MapEntry"));

            MinPick = 225;
            MineResist = 4.2f;

            HitSound = SoundID.Tink;
            DustType = DustID.Blood;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}