using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using CompTechMod.Common.Players;

namespace CompTechMod.Common.UI
{
    public class PyroUISystem : ModSystem
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            var pyro = player.GetModPlayer<PyrotechnicPlayer>();

            if (pyro.explosionFlashTimer > 0)
            {
                float alpha = pyro.explosionFlashTimer / 20f;

                spriteBatch.Draw(
                    TextureAssets.MagicPixel.Value,
                    new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
                    Color.White * alpha
                );
            }
        }
    }
}
