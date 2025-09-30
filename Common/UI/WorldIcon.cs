using Luminance.Core.MenuInfoUI;
using System.Collections.Generic;
using Terraria.ModLoader;
using CompTechMod.Common.Systems;

namespace CompTechMod.Common.UI
{
    public class WorldIcon : InfoUIManager
    {
        public override IEnumerable<WorldInfoIcon> GetWorldInfoIcons()
        {
            yield return new WorldInfoIcon(
                "CompTechMod/Content/Items/DontDoThis",       // путь к иконке (без .png)
                "Mods.CompTechMod.DontDoThisModeActive",      // ключ локализации
                header =>
                {
                    // Используем ваш ModSystem, который сохраняет тег
                    if (!header.TryGetHeaderData<CompWorld>(out var compWorld))
                        return false;

                    // Возвращаем состояние сложности из ModSystem
                    return CompWorld.DontDoThisMode;
                }, 
                1
            );
        }
    }
}
