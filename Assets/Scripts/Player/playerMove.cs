using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class playerMove : MonoBehaviour {

    [Header("Movement")]
    public float moveSpeed;
    public float jumpSpeed;
    public Transform orientation;

    public float groundDrag;

    KeyCode jumpKey = KeyCode.Space;
    float horizantalInput;
    float verticalInput;

    Vector3 moveDir;
    Boolean grounded;
    
    Rigidbody rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();
        getMovement();
        applyDrag();
        controlSpeed();

    }

    void FixedUpdate()
    {
        movePlayer();
        
    }

    private void jump()
    {
        if(!grounded) return;
        rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x,0,rigidbody.linearVelocity.z);

        rigidbody.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
    }

    private void controlSpeed()
    {
        Vector3 flatVelocity = new Vector3(rigidbody.linearVelocity.x,0,rigidbody.linearVelocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rigidbody.linearVelocity = new Vector3(limitedVelocity.x, rigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }



    private void checkGrounded()
    {
        //grounded = rigidbody.linearVelocity.y < 0.1f && rigidbody.linearVelocity.y >-0.1f ;
        grounded = rigidbody.linearVelocity.y == 0 ;
    }

    
    private void applyDrag()
    {
        if (grounded){
            rigidbody.linearDamping = groundDrag;
        } else
        {
           rigidbody.linearDamping = 0; 
        }
    }

    private void movePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizantalInput;

        rigidbody.AddForce(moveDir.normalized * moveSpeed, ForceMode.Force);
    }

    private void getMovement()
    {
        horizantalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        
        if (Input.GetKey(jumpKey) && grounded)
        {
            jump();
        }
    
        
    }
}
