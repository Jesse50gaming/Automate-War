using UnityEngine;

namespace Items
{
    public abstract class Item
    {
        private int count;
        private Vector2Int atlasCoord;

        public int Count => count;
        public Vector2Int AtlasCoord => atlasCoord;

        public Item(Vector2Int atlasCoord)
        {
            this.atlasCoord = atlasCoord;
            this.count = 0;
        }

        public void AddItem(int amount)
        {
            count += amount;
        }

        public void RemoveItem(int amount)
        {
            count -= amount;
            if (count < 0) count = 0;
        }
    }
}
