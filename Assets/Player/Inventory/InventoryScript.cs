using UnityEngine;
using Container;
public class InventoryScript : MonoBehaviour
{ 

    [SerializeField] int rows, columns;
    Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = new Inventory(rows, columns);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
