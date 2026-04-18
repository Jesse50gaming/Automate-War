using UnityEngine;
using Container;
public class InventoryScript : MonoBehaviour
{ 

     private const int rows = 3;
     private const int columns = 9;
    public Inventory inventory;
    public Hotbar hotbar;

    void Awake()
    {
        inventory = new Inventory(rows, columns);
        hotbar = new Hotbar();
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
