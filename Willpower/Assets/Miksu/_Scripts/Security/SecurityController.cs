using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityController : Draggable
{
    #region PROPERTIES

    #region AI Properties
    GameObject currentTarget;
    GameObject player;
    DoorManager doorManager;

    [Header("Moving")]
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float maxVelocity = 10f;

    private float jumpSeed = 1f;
    bool readyForJump = true;

    public enum aiMode
    {
        chase, goToDoor, idle, struggle, stunned, roam
    }
    public aiMode currentMode = aiMode.chase;
    private bool downed = false;
    private float timeBetweenDecisions = 1f;
    private bool readyForNewDecisions = true;

    float yAxisTreshold = 2f;

    private enum Orientation
    {
        left, right
    }
    //Orientation 
    Orientation currentOrientation = Orientation.left;
    float orientation = 1f;

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

    private void OnEnable()
    {
        // Restart the FlyTimer
        StartCoroutine(FlyTimer());

        ChangeMode(aiMode.chase);

        StartCoroutine(DecisionDelayer(1f));
        StartCoroutine(JumpDelay());
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
                FreezeRotation(false);

                // Keep the Layer on Flying as long as the timer hasn't run out

                while (flyTime > 0)
                {
                    //Debug.Log("Flytime: " + flyTime);

                    flyTime -= Time.deltaTime;

                    // UI
                    Highlight();

                    yield return new WaitForFixedUpdate();
                }

                ResetGuard();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ResetGuard()
    {
        // Reset rotation
        FreezeRotation(true);

        // Change the Layer back to Draggable
        ChangeLayer(Layer.idle);

        // Change the AI Mode to chase
        ChangeMode(aiMode.chase);
    }

    protected void FreezeRotation(bool onOff)
    {
        if (onOff == false) // Unfreeze rotation
        {
            rb.freezeRotation = false;

            // Give a little spin
            if (rb.angularVelocity < 0.1f) { rb.AddTorque(Random.Range(-0.2f, 0.2f), ForceMode2D.Impulse); }
        }
        else    // Reset rotation
        {
            rb.SetRotation(0f);
            rb.freezeRotation = true;
        }
    }
    #endregion

    #region HEALTH
    protected override void Highlight()
    {
        if (animator != null)
        {
            PlayAnimation(AnimationState.hover);

            // Change AI State
            ChangeMode(aiMode.struggle);
        }
    }

    public override void TakeDamage(float amount)
    {

        amount = Mathf.Ceil(amount);

        // UI - Spawn Hurt indicator
        if (hurtIndicator != null)
        {
            GameObject hurt = Instantiate(hurtIndicator, transform.position, transform.rotation) as GameObject;

            // Scale hurt UI depending on the amount
            float scale = Mathf.Min(1f, (0.5f + ((amount - velocityHurtLimit) * 0.1f)));

            hurt.transform.localScale = new Vector3(scale, scale, scale);

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

    private void CheckHealth()
    {
        if (health < 0f)
        {
            ChangeMode(aiMode.stunned);
        }
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

        // Find Door Manager
        doorManager = FindObjectOfType<DoorManager>(); if (!doorManager) { Debug.Log("No DoorManager found!"); }

        // Seed the jump
        jumpSeed = Random.Range(0.5f, 1f);
    }

    private void AI_Update()
    {
        //if (currentMode != aiMode.stunned || currentMode != aiMode.struggle)
        //{
        //    //Debug.Log("Current AiMode: " + currentMode.ToString());
        //    //CheckPlayerY_Pos();
        //}

        CheckMode();
    }

    private void ChangeMode(aiMode mode)
    {
        currentMode = mode;
    }

    private void CheckMode()
    {

        switch (currentMode)
        {
            case aiMode.idle:

                PlayAnimation(AnimationState.idle);

                break;

            // ========================

            case aiMode.chase:

                // Check Targeting
                CheckPlayerY_Pos();
                CheckPlayerXPos();


                // Check Health
                CheckHealth();

                // Animation
                PlayAnimation(AnimationState.run);

                MoveForwards();

                // Jump
                if (readyForJump) { DoJump(); StartCoroutine(JumpDelay()); }

                break;

            // ========================

            case aiMode.goToDoor:

                // Check Targeting
                CheckPlayerY_Pos();
                CheckCurrentTargetPos();

                // Check Health
                CheckHealth();

                // Animation
                PlayAnimation(AnimationState.run);

                // Move
                MoveForwards();

                // Check for doors --> HANDLED BY OnTriggerEnter()

                break;

            // ========================

            case aiMode.roam:

                Debug.Log("Roaming!");

                //ChangeMode(aiMode.chase);

                break;

            // ========================

            case aiMode.struggle:

                // Animation
                PlayAnimation(AnimationState.hover);

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

    private void CheckPlayerXPos()
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

    private void CheckCurrentTargetPos()
    {
        //if (currentTarget.transform.position.y > (transform.position.y + yAxisTreshold))
        //{
        //    // If The Door is too high, fetch a new door target
        //    //currentTarget = ;
        //}
        //else
        {
            // Keep chasing, check X-coord
            if (currentOrientation == Orientation.left)
            {
                // Check need to TURN to RIGHT
                if (currentTarget.transform.position.x > transform.position.x)
                {
                    // Turn
                    TurnAfterDelay(Orientation.right);
                }
            }
            else
            {
                // Check the need to TURN to LEFT
                if (currentTarget.transform.position.x < transform.position.x)
                {
                    // Turn
                    TurnAfterDelay(Orientation.left);
                }
            }
        }
    }

    protected void CheckPlayerY_Pos()
    {
        // Check if decisions are timely

        // Check if Player is ABOVE
        if (player.transform.position.y > (transform.position.y + yAxisTreshold))
        {
            if (!readyForNewDecisions) { return; }

            Debug.Log("Looking for door!");

            // Player is too high! Look for nearest door!
            currentMode = aiMode.goToDoor;
            currentTarget = doorManager.GoToNearestDoor(transform.position);

            StartCoroutine(DecisionDelayer(Random.Range(3f, 5f)));
        }
        // Check if the Player is BELOW
        else if (player.transform.position.y < (transform.position.y - yAxisTreshold))
        {
            if (!readyForNewDecisions) { return; }

            // Player is not on this level
            // --> Either ROAM
            //                  OR
            //                      GoToDoor

            #region Roam or GoTODoor
            // Check that the decisions isn't made already
            //if (currentMode != aiMode.goToDoor || currentMode != aiMode.roam)
            //{
            //    int randNum = Random.Range(0, 3);
            //    if (randNum == 0)
            //    {
            //        // Go to Door
            //        currentMode = aiMode.goToDoor;
            //    }
            //    else
            //    {
            //        // Roam
            //        currentMode = aiMode.roam;
            //    }
            //}
            #endregion

        }
        // If the Player is ON THE SAME LEVEL
        else
        {
            // Chase!
            currentMode = aiMode.chase;

            if (!readyForNewDecisions) { return; }
            StartCoroutine(DecisionDelayer(Random.Range(2f, 4f)));
        }
    }


    private IEnumerator DecisionDelayer(float time)
    {
        readyForNewDecisions = false;

        // Wait
        yield return new WaitForSeconds(time);

        // Activate decision making again
        readyForNewDecisions = true;
    }

    #region Turning
    private void TurnAfterDelay(Orientation direction)
    {
        //Debug.Log("Turning to: " + direction);

        float time = Random.Range(0.2f, 0.6f);

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
    private void MoveForwards()
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
        rb.velocity = new Vector2(Mathf.Max(rb.velocity.x, -maxVelocity), rb.velocity.y);

    }

    private void DoJump()
    {
        // Jump!
        rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }

    private IEnumerator JumpDelay()
    {
        // Regain jump stamina
        readyForJump = false;
        yield return new WaitForSeconds(jumpSeed + Random.Range(0.2f, 1f));

        readyForJump = true;
    }


    public bool DoorTrigger(Vector3 pos)
    {
        // Check if the Guard is looking for a door
        if (currentMode != aiMode.goToDoor)
        {
            return false;
        }

        // Check if Door is current Target
        if (currentTarget.transform.position == pos)
        {
            return true;
        }

        return false;
    }
    #endregion

    #endregion

}
