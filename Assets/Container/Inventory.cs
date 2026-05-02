using UnityEngine;
using Items;
using System;

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
            if(row > rows || row <0 || column > columns || column <0) return null;
            Item item2 = inventory[row,column];
            inventory[row,column] = item;

            return item2;
        }

        public void removeItem(int row, int column)
        {
            if(row > rows || row <0 || column > columns || column <0) return;
            inventory[row,column] = null;
        }
    }
}

