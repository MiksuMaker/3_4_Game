using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    #region PROPERTIES
    protected Dragger dragger;
    protected Animator animator;



    [Header("Health")]
    [SerializeField]
    protected float velocityHurtLimit = 5f;
    [SerializeField]
    protected float MAXhealth = 10;
    [SerializeField]
    protected float health;
    [SerializeField]
    public GameObject hurtIndicator;

    [Header("Physics")]
    [SerializeField]
    float mass = 1f;
    [SerializeField]
    protected float velocityLimit = 5f;
    Vector2 currentVelocity;

    public enum Layer
    {
        idle, flying
    }
    public Layer currentLayer = Layer.idle;
    protected float flyTime;


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
    protected void Start()
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

        // Start the FlyTImer
        StartCoroutine(FlyTimer());

    }

    protected virtual void FixedUpdate()
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

    public virtual void InitializeDrag()
    {
        ChangeLayer(Layer.flying);
    }

    public virtual void DragMeAround(Vector2 mousePos, float dragForce)
    {
        // Get the distance between mouse and this object


        // Add the Force
        rb.AddForce(GetDirection(mousePos) * dragForce, ForceMode2D.Force);



        // Keep the timer running
        flyTime = 1f;
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

        return /* DIRECTION: */ (mousePos - thisPos); // Damps with distance
        //return /* DIRECTION: */ (mousePos - thisPos).normalized;  // Normalized
    }
    #endregion

    #region ANIMATIONS
    protected virtual void Highlight()
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
            Draggable obj;
            if (collision.gameObject.TryGetComponent<Draggable>(out obj))
            {
                obj.TakeDamage(rb.velocity.magnitude);
            }

            // Damage yourself too
            TakeDamage(rb.velocity.magnitude);
        }
    }

    protected virtual void ChangeLayer(Layer layer)
    {
        switch (layer)
        {
            case Layer.idle:
                gameObject.layer = LayerMask.NameToLayer("Draggable");

                break;
            // ======================
            case Layer.flying:
                gameObject.layer = LayerMask.NameToLayer("Flying");

                break;
        }
    }

    protected virtual IEnumerator FlyTimer()
    {
        while (true)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Flying"))
            {


                // Keep the Layer on Flying as long as the timer hasn't run out

                while (flyTime > 0)
                {
                    flyTime -= Time.deltaTime;

                    // UI
                    Highlight();

                    yield return new WaitForFixedUpdate();
                }

                // Change the Layer back to Draggable
                ChangeLayer(Layer.idle);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion


    #region DAMAGE
    public virtual void TakeDamage(float amount)
    {
        //Debug.Log("Damage taken: " + amount);

        amount = Mathf.Ceil(amount);

        // UI - Spawn Hurt indicator
        if (hurtIndicator != null)
        {
            GameObject hurt = Instantiate(hurtIndicator, transform.position, transform.rotation) as GameObject;

            // Scale according to damage taken
            //hurt.transform.localScale = new Vector3();



            Destroy(hurt, 1f);
        }

        // Take damage
        health -= amount;

        // Check if destroyed
        if (health <= 0f)
        {
            BeDestroyed();
        }
    }

    protected virtual void BeDestroyed()
    {
        // Play animation
        if (hurtIndicator != null)
        {
            GameObject hurt = Instantiate(hurtIndicator, transform.position, transform.rotation) as GameObject;
            hurt.transform.localScale = new Vector3(1.5f, 1.5f);
            Destroy(hurt, 1f);
        }

        // Die
        Destroy(gameObject);
    }
    #endregion
}
