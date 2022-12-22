using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityController : Draggable
{
    #region PROPERTIES

    #region AI Properties
    GameObject currentTarget;
    GameObject player;

    [Header("Moving")]
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float maxVelocity = 10f;

    public enum aiMode
    {
        chase, goToDoor, idle, struggle, stunned
    }
    public aiMode currentMode = aiMode.chase;
    private bool downed = false;

    private enum Orientation
    {
        left, right
    }
    //Orientation 
    Orientation currentOrientation = Orientation.left;
    float orientation = 1f;

    float yAxisTreshold = 2f;
    #endregion


    GameObject graphics;

    protected enum AnimationState
    {
        idle, run, hover, hurt
    }
    AnimationState currentState = AnimationState.idle;
    #endregion

    #region BUILTIN
    void Start()
    {
        base.Start();

        // Get references
        graphics = animator.gameObject;

        AI_Start();
    }




    protected virtual void FixedUpdate()
    {

        #region Old
        if (rb.velocity.magnitude > velocityLimit)
        {
            // Check if object is still in Drag

            if (dragger.CheckObjectsInPullList(this as Draggable))
            {
                // Limit the velocity
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, velocityLimit);
            }
            else
            {
                // Not in drag
                // -->  Don't do a thing
                //Debug.Log("Freefalling");
            }
        }
        #endregion


        // AI Update
        AI_Update();
    }
    #endregion

    #region DRAGGABLE OVERRIDES
    protected virtual void ChangeLayer(Layer layer)
    {
        switch (layer)
        {
            case Layer.idle:
                gameObject.layer = LayerMask.NameToLayer("DraggableCharacter");

                break;
            // ======================
            case Layer.flying:
                gameObject.layer = LayerMask.NameToLayer("Flying");

                break;
        }
    }

    protected override IEnumerator FlyTimer()
    {
        while (true)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Flying"))
            {
                // Release the freeze on rotation
                RotationOnOff(false);
                
                // Keep the Layer on Flying as long as the timer hasn't run out

                while (flyTime > 0)
                {
                    flyTime -= Time.deltaTime;

                    // UI
                    Highlight();

                    yield return new WaitForFixedUpdate();
                }

                // Reset rotation
                RotationOnOff(true);

                // Change the Layer back to Draggable
                ChangeLayer(Layer.idle);

                // Change the AI Mode to chase
                ChangeMode(aiMode.chase);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected void RotationOnOff(bool onOff)
    {
        if (onOff == false) // Unfreeze rotation
        {
            rb.freezeRotation = false;
            // Give a little spin
            if (rb.angularVelocity < 0.1f) { rb.AddTorque(Random.Range(-0.2f, 0.2f), ForceMode2D.Impulse); }

        }
        else    // Reset rotation
        {
            //StartCoroutine(ResetRotation());
            rb.SetRotation(0f);
            rb.freezeRotation = true;
        }
    }

    IEnumerator ResetRotation()
    {
        WaitForSeconds interval = new WaitForSeconds(0.1f);
        float getUpTime = 1f;
        float timePassed = 0f;

        while(timePassed <= getUpTime)
        {
            // Rotate the character upright
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                 Quaternion.Euler(0f, 0f, 0f),
                                                 timePassed);

            timePassed += Time.deltaTime;

            yield return interval;
        }
    }

    protected override void Highlight()
    {
        if (animator != null)
        {
            PlayAnimation(AnimationState.hover);

            // Change AI State
            ChangeMode(aiMode.struggle);
        }
    }

    protected override void TakeDamage(float amount)
    {
        // UI - Spawn Hurt indicator
        if (hurtIndicator != null)
        {
            GameObject hurt = Instantiate(hurtIndicator, transform.position, transform.rotation) as GameObject;
            Destroy(hurt, 1f);
        }


        // Take damage
        health -= amount;

        // Trigger hurt animation
        dragger.UndragMe(this as Draggable);

        // Check if destroyed
        if (health < 0f)
        {
            //Debug.Log("Health: " + health);
            BeDestroyed();
        }
    }

    protected override void BeDestroyed()
    {
        downed = true;
        ChangeMode(aiMode.stunned);
    }
    #endregion

    #region ANIMATIONS
    private void PlayAnimation(AnimationState state)
    {
        currentState = state;

        // Check correct orientation
        CheckOrientation();

        switch (state)
        {
            case AnimationState.idle:

                animator.Play("Guard_Idle");

                break;

            // =========================

            case AnimationState.run:

                animator.Play("Guard_Run");

                break;

            // =========================

            case AnimationState.hover:

                animator.Play("Guard_Struggle");

                break;

            // =========================

            case AnimationState.hurt:

                animator.Play("Guard_Down");

                break;

            // =========================
            default:
                break;

        }
    }

    private void CheckOrientation()
    {
        if (currentOrientation == Orientation.left)
        { orientation = 1f; }
        else { orientation = -1f; }

        // LEFT: +1,    RIGHT: -1
        Vector3 scale = new Vector3(orientation, 1f, 1f);

        // Set scale
        graphics.transform.localScale = scale;
    }
    #endregion

    #region AI
    private void AI_Start()
    {
        // Find Player
        player = FindObjectOfType<ProtagController>().gameObject;
        currentTarget = player;
    }

    private void AI_Update()
    {
        CheckTargeting();
    }

    private void ChangeMode(aiMode mode)
    {
        currentMode = mode;
    }

    private void CheckTargeting()
    {

        switch (currentMode)
        {
            case aiMode.idle:

                break;

            // ========================

            case aiMode.chase:

                // Check Health
                CheckHealth();

                // Animation
                PlayAnimation(AnimationState.run);

                CheckPlayerPos();
                MoveTowardsTarget();

                break;

            // ========================

            case aiMode.goToDoor:

                // Check Health
                CheckHealth();

                // Animation
                PlayAnimation(AnimationState.run);

                // Move
                MoveTowardsTarget();

                // Check for doors

                break;

            // ========================

            case aiMode.stunned:

                // Animation
                PlayAnimation(AnimationState.hurt);

                break;

            // ========================

            default:
                break;
        }
    }

    private void CheckPlayerPos()
    {
        // Check Player Y-coordinate

        //if (player.transform.position.y > (transform.position.y + yAxisTreshold))
        //{
        //    // If Player is too high, go to nearest door
        //    currentMode = aiMode.goToDoor;
        //}
        //else
        {
            // Keep chasing, check X-coord
            if (currentOrientation == Orientation.left)
            {
                // Check need to TURN to RIGHT
                if (player.transform.position.x > transform.position.x)
                {
                    // Turn
                    TurnAfterDelay(Orientation.right);
                }
            }
            else
            {
                // Check the need to TURN to LEFT
                if (player.transform.position.x < transform.position.x)
                {
                    // Turn
                    TurnAfterDelay(Orientation.left);
                }
            }
        }
    }

    #region Turning
    private void TurnAfterDelay(Orientation direction)
    {
        //Debug.Log("Turning to: " + direction);

        float time = Random.Range(0.1f, 0.5f);

        StartCoroutine(TimedTurn(direction, time));
    }

    IEnumerator TimedTurn(Orientation direction, float delay)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        yield return wait;

        // Execute Turn
        currentOrientation = direction;
        CheckOrientation();
    }
    #endregion

    #region MOVING
    private void MoveTowardsTarget()
    {
        // Move forwards
        Vector2 forwards;
        if (currentOrientation == Orientation.left)
        {
            forwards = new Vector2(-1f, 0f);    // Left
        }
        else
        {
            forwards = new Vector2(1f, 0f);     // Right
        }

        Debug.DrawLine(transform.position, new Vector3(transform.position.x,
                                                transform.position.y - 0.2f,
                                                transform.position.z), Color.red);

        // If Guard is touching ground
        if (Physics2D.OverlapCircle(new Vector3(transform.position.x,
                                                transform.position.y - 0.2f,
                                                transform.position.z),
                                    0.5f, LayerMask.GetMask("Default")))
        {
            // Move
            rb.AddForce(forwards * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        // Limit velocity on X-axis
        rb.velocity = new Vector2(Mathf.Min(rb.velocity.x, maxVelocity), rb.velocity.y);

    }

    private void MoveAndCheckDoors()
    {
        if (!(player.transform.position.y > (transform.position.y + yAxisTreshold)))
        {
            // If Player is too high, go to nearest door
            currentMode = aiMode.chase;
        }
        else
        {
            // Keep chasing, check X-coord
            if (currentOrientation == Orientation.left)
            {
                // Check need to TURN to RIGHT
                if (player.transform.position.x > transform.position.x)
                {
                    // Turn
                    TurnAfterDelay(Orientation.right);
                }
            }
            else
            {
                // Check the need to TURN to LEFT
                if (player.transform.position.x < transform.position.x)
                {
                    // Turn
                    TurnAfterDelay(Orientation.left);
                }
            }
        }
    }
    #endregion

    #endregion

    private void CheckHealth()
    {
        if (health < 0f)
        {
            ChangeMode(aiMode.stunned);
        }
    }
}
