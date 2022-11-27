using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{






    public bool CanMove = false;

    [Header("Movement")]
    public float movementSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool isGrounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 movementDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
    }
    void Update()
    {


        // ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        MyInput();


        // handle drag
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }

        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && isGrounded && CanMove)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {

        if (!CanMove) { return; }

        // calculate movement direction
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (isGrounded) rb.AddForce(movementDirection.normalized * movementSpeed * 10f, ForceMode.Force);

        // in air
        else if (!isGrounded) rb.AddForce(movementDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump()
    {
        readyToJump = true;
    }




 

}
