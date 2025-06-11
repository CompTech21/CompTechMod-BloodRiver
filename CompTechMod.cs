using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using CompTechMod.Common.Systems;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.Liquid;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace CompTechMod
{
	public class CompTechMod : Mod
	{
		public static CompTechMod Instance;
		
		public CompTechMod()
		{
			Instance = this;
		}
	}
}
