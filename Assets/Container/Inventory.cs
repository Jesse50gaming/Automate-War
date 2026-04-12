using UnityEngine;
using Items;

namespace Container
{
    public class Inventory
    {

        public Item[,] inventory;

        public int rows, columns;

        
        public Inventory(int rows, int columns)
        {   
            this.rows = rows;
            this.columns = columns;

            inventory = new Item[rows,columns];
        }


        public Item getItem(int row, int column)
        {
            return inventory[row,column];
        }

        public Item putItem(int row, int column, Item item)
        {
            Item item2 = inventory[row,column];
            inventory[row,column] = item;

            return item2;
        }


    }
}

