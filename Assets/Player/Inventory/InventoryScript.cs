using UnityEngine;
using Container;
public class InventoryScript : MonoBehaviour
{ 

     private const int rows = 4;
     private const int columns = 10;
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
