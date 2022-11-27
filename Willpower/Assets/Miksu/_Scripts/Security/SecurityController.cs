using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityController : Draggable
{
    #region PROPERTIES


    GameObject graphics;

    protected enum AnimationState
    {
        idle, run, hover, hurt
    }
    AnimationState currentState = AnimationState.idle;
    float orientation = 1f;
    #endregion

    #region BUILTIN
    void Start()
    {
        base.Start();

        // Get references
        graphics = animator.gameObject;

        Debug.Log("Animator: " + animator.runtimeAnimatorController.name);
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

    protected override void Highlight()
    {
        if (animator != null)
        {
            PlayAnimation(AnimationState.hover);
        }
    }

    protected override void TakeDamage(float amount)
    {
        // Take damage
        health -= amount;

        // Trigger hurt animation
        dragger.UndragMe(this as Draggable);

        // Check if destroyed
        if (health <= 0f)
        {
            BeDestroyed();
        }
    }

    protected override void BeDestroyed()
    {
        PlayAnimation(AnimationState.hurt);
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
        // 'Orientation' variable is modified in CheckMoveHorizontal()

        // LEFT: +1,    RIGHT: -1
        Vector3 scale = new Vector3(orientation, 1f, 1f);

        // Set scale
        graphics.transform.localScale = scale;
    }
    #endregion
}
