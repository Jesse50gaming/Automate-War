using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public float sensX;
    public float sensY;
    public Transform orientation;
    
    float xRotation;
    float yRotation;

    public bool moving = true;
    [SerializeField] private InventoryGUIScript inventoryGUIScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (!moving) return;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        
        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation,-90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation,0);
    }

    public void setMovement(bool move)
    {
        moving = move;

        if (moving)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
   
}
