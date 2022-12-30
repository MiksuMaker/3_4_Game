using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagController : MonoBehaviour
{
    #region PROPERTIES
    bool selfTelekinesisOn = false;


    // Movement
    [Header("Movement")]
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

    // ANIMATION
    Animator animator;
    GameObject graphics;
    private enum AnimationState
    {
        idle, run, hover, hurt
    }
    AnimationState currentAnimation = AnimationState.idle;
    int orientation = 1; // 1 is LEFT, -1 is RIGHT

    //[Header("Health")]

    #endregion

    #region BUILTIN
    private void Start()
    {
        // Get references
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        graphics = animator.gameObject;
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

            case GameMode.hurt:

                // Restart game?

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
                // Play Animation
                PlayAnimation(AnimationState.hover);

                // Switch
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
            orientation = 1;
            inputsThisFrame = true;
        }
        // D
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += new Vector2(1f, 0f);
            orientation = -1;
            inputsThisFrame = true;
        }

        // Check if any inputs
        if (moveDir.x != 0f)
        {
            // Play RUN animation
            PlayAnimation(AnimationState.run);

            MoveHorizontal();
        }
        else
        {
            // Play IDLE Animation
            PlayAnimation(AnimationState.idle);
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

        #region Orientation
        if (nextPos.x < transform.position.x)
        { orientation = 1; }        // Left
        else { orientation = -1; }  // Right
        CheckOrientation();
        #endregion

        Debug.DrawLine(transform.position, nextPos);
        // Clamp
        //nextPos = Vector2.ClampMagnitude(nextPos, maxHoverSpeed);
        nextPos = Vector2.MoveTowards(transform.position, nextPos, maxHoverSpeed);

        //Debug.Log("NextPos: " + nextPos);

        // Lerp the Protag towards mouse
        rb.MovePosition(nextPos);
    }

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

    #endregion


    #region ANIMATIONS
    private void PlayAnimation(AnimationState state)
    {
        currentAnimation = state;


        // Check correct orientation
        CheckOrientation();

        switch (state)
        {
            case AnimationState.idle:

                animator.Play("Protag_Idle");

                break;

            // =========================

            case AnimationState.run:

                animator.Play("Protag_Run");

                break;

            // =========================

            case AnimationState.hover:

                animator.Play("Protag_Hover");

                break;

            // =========================

            case AnimationState.hurt:

                animator.Play("Protag_Hurt");

                break;

            // =========================
            default:
                break;

        }
    }

    private void CheckOrientation()
    {
        // 'Orientation' variable is modified in CheckMoveHorizontal()

        // LEFT: +1,    RIGHT: -1
        Vector3 scale = new Vector3(orientation, 1f, 1f);

        // Set scale
        graphics.transform.localScale = scale;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if Guard
        if (collision.gameObject.layer == LayerMask.NameToLayer("DraggableCharacter"))
        {
            if (collision.gameObject.GetComponent<SecurityController>().currentMode != SecurityController.aiMode.stunned)
            {
                currentMode = GameMode.hurt;
                PlayAnimation(AnimationState.hurt);
            }
        }
    }
}
