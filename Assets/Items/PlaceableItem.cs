using UnityEngine;

namespace Items
{
    public abstract class PlaceableItem : Item
    {
        protected BlockType block;

        public PlaceableItem(BlockType block, Vector2Int atlasCoord,int count) : base(atlasCoord,count)
        {
            this.block = block;
        }

        public BlockType GetBlock()
        {
            return block;
        }
    }
}
