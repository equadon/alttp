using System.Collections.Generic;

namespace Alttp.Core.GameObjects.Interfaces
{
    public interface ILootable
    {
        List<GameObject> LootInventory { get; }

        GameObject[] Loot();
    }
}
