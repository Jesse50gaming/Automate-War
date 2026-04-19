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

        private Inventory hotbarInventory;
        
        
        public Hotbar()
        {
            hotbarInventory = new Inventory(hotbarHeight,hotbarWidth);
            hotbarInventory.putItem(0,0,new DirtItem(1));
        }


        public Item getItem(int slot)
        {
            if (slot < 0 || slot > hotbarWidth) return null;

            return hotbarInventory.getItem(hotbarHeight,slot);
        }

        public bool hasItem(int slot)
        {
            if (slot < 0 || slot > hotbarWidth) return false;
            if (hotbarInventory.getItem(hotbarHeight, slot) != null) return true;
            return false;
            
        }


        public void addItem(int slot,Item item)
        {
            hotbarInventory.putItem(hotbarHeight, slot, item);
        }

    }
}

