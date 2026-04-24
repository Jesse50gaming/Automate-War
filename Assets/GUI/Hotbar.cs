using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Items;
using Container;
using UnityEngine.Rendering.Universal.Internal;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Container
{
    [System.Serializable]
    public class Hotbar
    {
        public const int hotbarHeight = 1;
        public const int hotbarWidth = 10;
        public static readonly Vector2 hotbarSize = new Vector2(171,18);
        public const int slotWidth = 16;
        public const int slotPadding = 1;
        public const int gapFromBottomOfScreen = 10;
        private Inventory hotbarInventory;

        private HotbarScript hotbarScript;
        
        
        public Hotbar(HotbarScript hotbarScript)
        {
            this.hotbarScript = hotbarScript;
            hotbarInventory = new Inventory(hotbarHeight,hotbarWidth);
            hotbarInventory.putItem(hotbarHeight-1,0,new DirtItem(1));
            hotbarInventory.putItem(hotbarHeight-1,1,new StoneItem(1));
            hotbarInventory.putItem(hotbarHeight-1,2,new DirtItem(1));
            hotbarInventory.putItem(hotbarHeight-1,3,new StoneItem(1));
            hotbarInventory.putItem(hotbarHeight-1,4,new DirtItem(1));
            hotbarInventory.putItem(hotbarHeight-1,5,new StoneItem(1));
            hotbarInventory.putItem(hotbarHeight-1,6,new DirtItem(1));
            hotbarInventory.putItem(hotbarHeight-1,7,new StoneItem(1));
            hotbarInventory.putItem(hotbarHeight-1,8,new DirtItem(1));
            hotbarInventory.putItem(hotbarHeight-1,9,new StoneItem(1));
            
        }


        public Item getItem(int slot)
        {
            if (slot < 0 || slot > hotbarWidth) return null;

            return hotbarInventory.getItem(hotbarHeight-1,slot);
        }

        public bool hasItem(int slot)
        {
            if (slot < 0 || slot > hotbarWidth) return false;
            if (hotbarInventory.getItem(hotbarHeight-1, slot) != null) return true;
            return false;
            
        }


        public void addItem(int slot,Item item)
        {
            if (slot < 0 || slot > hotbarWidth) return;
            hotbarInventory.putItem(hotbarHeight-1, slot, item);
            hotbarScript.updateHotbar();
        }

        public Item takeItem(int slot)
        {
            if (slot < 0 || slot > hotbarWidth) return null;

            Item item = hotbarInventory.getItem(hotbarHeight-1,slot);
            hotbarInventory.removeItem(hotbarHeight-1,slot);
            hotbarScript.updateHotbar();
            return item;
        }

    }
}

