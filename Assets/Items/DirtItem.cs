using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public class DirtItem : PlaceableItem
    {
        
        public DirtItem(int count) : base(BlockType.DIRT, ItemAtlasCoords.DIRT,count)
        {
            
        }

    }
}