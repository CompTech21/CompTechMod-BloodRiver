using Terraria;
using Terraria.ModLoader;

#nullable disable
namespace CompTechMod.Common.Systems;

public class SpawnRateSystem : GlobalNPC
{
  public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
  {
    if (!Main.bloodMoon)
      return;
    spawnRate /= 10;
    maxSpawns *= 7;
  }
}
