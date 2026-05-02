using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using UnityEngine;

namespace Items
{
    public class GrassItem : PlaceableItem
    {
        
        public GrassItem(int count) : base(BlockType.GRASS, ItemAtlasCoords.GRASS,count)
        {
            
        }

    }
}