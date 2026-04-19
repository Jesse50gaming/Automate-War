using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public class StoneItem : PlaceableItem
    {
        
        public StoneItem(int count) : base(BlockType.STONE, ItemAtlasCoords.STONE,count)
        {
            
        }

    }
}