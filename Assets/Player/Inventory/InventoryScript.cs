using UnityEngine;
using Container;
public class InventoryScript : MonoBehaviour
{ 

    public const int rows = 4;
    public const int columns = 10;
    public readonly Vector2 inventorySize = new Vector2(171, 69);
    public const int distanceFromFloor = 200;
    public Inventory inventory;
    public Hotbar hotbar;
    [SerializeField] private HotbarScript hotbarScript;

    void Awake()
    {
        inventory = new Inventory(rows, columns);
        hotbar = new Hotbar(hotbarScript);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
