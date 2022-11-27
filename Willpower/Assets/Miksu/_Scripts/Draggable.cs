using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    #region PROPERTIES
    Dragger dragger;
    Animator animator;


    [Header("Health")]
    [SerializeField]
    float velocityHurtLimit = 5f;
    [SerializeField]
    float MAXhealth = 10;
    [SerializeField]
    float health;

    [Header("Physics")]
    [SerializeField]
    float mass = 1f;
    [SerializeField]
    float velocityLimit = 5f;
    Vector2 currentVelocity;


    public Rigidbody2D rb;

    [Header("Dragging")]
    bool beingDragged = false;
    [SerializeField]
    float maxDistance = 2f;
    [SerializeField]
    float minDistance = 0.1f;

    float dragTick = 0.1f;
    #endregion

    #region BUILTIN
    private void Start()
    {
        // Try to find Dragger
        dragger = FindObjectOfType<Dragger>();

        // Set up the 2D-Rigidbody
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass;

        // Find the Animator
        animator = GetComponentInChildren<Animator>();

        // Set health
        health = MAXhealth;

    }

    private void FixedUpdate()
    {
        // Limit the MAX Velocity
        //if (rb.velocity.magnitude > velocityLimit)
        //{
        //    // Limit the velocity
        //    rb.velocity = Vector2.ClampMagnitude(rb.velocity, velocityLimit);
        //}

        #region Old
        if (rb.velocity.magnitude > velocityLimit)
        {
            // Check if object is still in Drag

            if (dragger.CheckObjectsInPullList(this))
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

    #region DRAGGING
    public void UpdateDraggingStatus(bool isBeingDragged)
    {
        beingDragged = isBeingDragged;
    }

    public void DragMeAround(Vector2 mousePos, float dragForce)
    {
        // Get the distance between mouse and this object


        // Add the Force
        //AddAccordingForce(mousePos, dragForce);
        rb.AddForce(GetDirection(mousePos) * dragForce, ForceMode2D.Force);
        //rb.AddForce(GetDirection(mousePos) * dragForce, ForceMode2D.Impulse);

        Highlight();
    }

    private void AddAccordingForce(Vector2 mousePos, float dragForce)
    {
        if (rb.velocity.magnitude <= 0.1f)
        {
            // Add startup force
            rb.AddForce(GetDirection(mousePos) * dragForce, ForceMode2D.Impulse);

        }
        else
        {
            // Add slow continuous force
            rb.AddForce(GetDirection(mousePos) * dragForce, ForceMode2D.Force);

        }
    }

    #endregion

    #region CALCULATIONS
    private float GetDistance(Vector2 mousePos)
    {
        // Calculate distance between this and mouse

        return /*DISTANCE: */ Vector2.Distance(transform.position, mousePos);
    }

    private Vector2 GetDirection(Vector2 mousePos)
    {
        // Calculate the direction to be dragged towards
        Vector2 thisPos = transform.position;

        return /* DIRECTION: */ (mousePos - thisPos);
    }
    #endregion

    #region ANIMATIONS
    private void Highlight()
    {
        if (animator != null)
        {
            animator.Play("Highlight");
        }
    }
    #endregion

    #region COLLISIONS
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Velocity: " + rb.velocity.magnitude);

        // Test how hard you hit something
        if (rb.velocity.magnitude >= velocityHurtLimit)
        {
            // Try to damage the other object


            // Damage yourself too
            TakeDamage(rb.velocity.magnitude);
        }
    }
    #endregion


    #region DAMAGE
    private void TakeDamage(float amount)
    {
        Debug.Log("Damage taken: " + amount);

        // Take damage
        health -= amount;

        // Check if destroyed
        if (health <= 0f)
        {
            BeDestroyed();
        }
    }

    private void BeDestroyed()
    {
        // Play animation

        // Die
        Destroy(gameObject);
    }
    #endregion
}
