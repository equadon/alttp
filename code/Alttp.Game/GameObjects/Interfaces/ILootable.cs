using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.GameObjects
{
    public interface ILootable
    {
        List<GameObject> LootInventory { get; }

        GameObject[] Loot();
    }
}
