using UnityEngine;

namespace Items
{
    public abstract class Item
    {
        private int count;
        private Vector2Int atlasCoord;

        protected int maxCount = 100;

        public Item(Vector2Int atlasCoord, int count)
        {
            this.atlasCoord = atlasCoord;
            this.count = count;
        }

        public int AddItem(int amount)
        {
            if (count + amount > maxCount)
            {
                count = maxCount;
                return count + amount - maxCount;
            }
            count += amount;
            return 0;
        }

        public void RemoveItem(int amount)
        {
            count -= amount;
            if (count < 0) count = 0;
        }

        public Vector2Int getAtlasCoords()
        {
            return atlasCoord;
        }

        public int getCount()
        {
            return count;
        }

        public void setCount(int count)
        {
            this.count = count;
        }
        
    }
}
