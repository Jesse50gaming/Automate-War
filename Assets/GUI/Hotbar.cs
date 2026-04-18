using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Items;
using Container;
using UnityEngine.Rendering.Universal.Internal;
using Unity.Burst.CompilerServices;

namespace Container
{
    [System.Serializable]
    public class Hotbar
    {
        public const int hotbarHeight = 0;
        public const int hotbarWidth = 9;

        private Inventory inventory;
        
        
        public Hotbar()
        {
            inventory = new Inventory(hotbarHeight,hotbarWidth);
        }


        public Item getItem(int slot)
        {
            if (slot < 0 || slot > hotbarWidth) return null;

            return inventory.getItem(hotbarHeight,slot);
        }

    }
}

