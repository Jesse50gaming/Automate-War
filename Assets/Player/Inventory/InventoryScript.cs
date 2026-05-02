using UnityEngine;
using Container;
using Items;
public class InventoryScript : MonoBehaviour
{ 

    public const int rows = 4;
    public const int columns = 10;
    public readonly Vector2 inventorySize = new Vector2(171, 69);
    public const int distanceFromFloor = 200;
    public Inventory inventory;
    public Hotbar hotbar;
    [SerializeField] private HotbarScript hotbarScript;
    [SerializeField] private InventoryGUIScript inventoryGUIScript;

    void Awake()
    {
        inventory = new Inventory(rows, columns);
        hotbar = new Hotbar(hotbarScript);
    }

    // Start is called once after all Awake methods have run
    void Start()
    {
        // Delay GUI drawing until the inventory UI has finished creating its slot objects.
        putItem(new DirtItem(5), 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void putItem(Item item, int row, int col)
    {
        inventory.putItem(row, col, item);
        inventoryGUIScript.DrawItem(item, row, col);
    }

    
}
