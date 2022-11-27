using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagController : MonoBehaviour
{
    #region PROPERTIES
    [Header("Gamemode")]
    bool selfTelekinesisOn = false;


    // Movement
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float maxVelocity = 10f;

    Vector2 moveDir = Vector2.zero;
    Rigidbody2D rb;
    GameMode currentMode = GameMode.horizontal;

    // Hovering
    [Header("Hovering")]
    [SerializeField]
    float hoverSpeed = 0.5f;
    [SerializeField]
    float maxHoverSpeed = 15f;

    Vector2 lastPos = Vector2.zero;

    private enum GameMode
    {
        horizontal, selfTelekinesis, hurt, dead
    }

    private enum ProtagAnimation
    {
        idle, run, hover, hurt
    }
    #endregion

    #region BUILTIN
    private void Start()
    {
        // Get references
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckInput();
    }
    #endregion

    #region SETUP

    #endregion


    #region INPUT
    private void CheckInput()
    {
        switch (currentMode)
        {
            case GameMode.horizontal:

                CheckMoveHorizontal();

                CheckModeSwitch();

                break;

            // =====================

            case GameMode.selfTelekinesis:

                DragProtag();
                CheckModeSwitch();

                break;

            // ======================

            default:
                // No game mode
                break;
        }

    }

    private void CheckModeSwitch()
    {
        // Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Toggle selfTelekinesis on/off

            selfTelekinesisOn = !selfTelekinesisOn;

            // Switch mode
            if (selfTelekinesisOn)
            {
                currentMode = GameMode.selfTelekinesis;
            }
            else
            {
                // Carry over the momentum
                InheritHoverMomentum();

                // Switch
                currentMode = GameMode.horizontal;
            }
        }
    }

    private void CheckMoveHorizontal()
    {
        bool inputsThisFrame = false;

        // A
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += new Vector2(-1f, 0f);
            inputsThisFrame = true;
        }
        // D
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += new Vector2(1f, 0f);
            inputsThisFrame = true;
        }

        // Check if any inputs
        if (moveDir.x != 0f)
        {
            MoveHorizontal();
        }
    }

    #endregion

    #region MOVEMENT
    private void MoveHorizontal()
    {
        Vector2 curPos = transform.position;

        // Move according to moveDir
        rb.AddForce(moveDir * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);

        // Limit velocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);



        // Clear moveDir
        moveDir = Vector2.zero;
    }
    #endregion

    #region DRAGGING
    private void PutMouseOnPlayer()
    {
        // This is too complicated apparently

        // --> just lerp the protag towards mouse instead
    }

    private void DragProtag()
    {
        // Get the direction towards the mouse
        //Vector2 pullDir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Lerp the currentPos towards that direction
        //Vector2 nextPos = Vector2.Lerp(transform.position, pullDir, hoverSpeed * Time.deltaTime);
        Vector2 nextPos = Vector2.Lerp(transform.position,
                                       GetMousePos(),
                                       hoverSpeed * Time.deltaTime);

        Debug.DrawLine(transform.position, nextPos);
        // Clamp
        nextPos = Vector2.ClampMagnitude(nextPos, maxHoverSpeed);

        //Debug.Log("NextPos: " + nextPos);

        // Lerp the Protag towards mouse
        //rb.MovePosition(nextPos);
        rb.MovePosition(nextPos);
    }
    #endregion

    private void InheritHoverMomentum()
    {
        // Get the difference between mousePos and currentPos
        Vector2 curPos = rb.position;
        Vector2 difference = GetMousePos() - curPos;

        // Give that difference as velocity to RB (times 2)
        rb.velocity = difference * 2f;
    }

    private Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
