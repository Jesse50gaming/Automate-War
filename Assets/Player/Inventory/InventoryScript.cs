using UnityEngine;
using Container;
using Items;
public class InventoryScript : MonoBehaviour
{ 
    [SerializeField]
    public const int rows = 4;
    [SerializeField]
    public const int columns = 10;

    public Inventory inventory;
    
    [SerializeField] private HotbarScript hotbarScript;

    void Awake()
    {
        inventory = new Inventory(rows, columns);
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item SwapItem(Item item, int row, int col)
    {

        Item item2 = inventory.SwapItem(row, col, item);
        return item2;
    }

    public Item getItem(int row, int col)
    {
        return inventory.getItem(row, col);
    }

    
}
