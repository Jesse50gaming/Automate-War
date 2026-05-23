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

        public Item SwapItem(int row, int column, Item item)
        {
            if(row > rows || row <0 || column > columns || column <0) return item;//if out of bounds, put item back where it was
            
            Item item2 = inventory[row,column];

            if (item.GetType() == item2.GetType()) // the items are the same type, merge
            {
                item.setCount(item2.AddItem(item.getCount())); // adds to item 2 gives remainder to item 1
                inventory[row, column] = item2;

                return item;
            }
            inventory[row,column] = item;

            return item2; // return item already in place // returns null if no item is there
        }

        
    }
}

