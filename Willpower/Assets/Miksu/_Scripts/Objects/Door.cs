using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] int floor = 0;
    float height;

    [SerializeField] LayerMask[] blockingLayers;

    Animator animator;
    #endregion

    #region BUILTIN
    private void Start()
    {
        // Cache height
        height = transform.position.y;

        // Get references
        animator = GetComponentInChildren<Animator>();
    }
    #endregion

    #region GUARD MOVING
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if other object is Guard
        SecurityController guard;
        if (collision.TryGetComponent(out guard))
        {

            // Check if this is the Door they are targeting
            if (guard.DoorTrigger(transform.position))
            {
                Debug.Log("Guard trying to enter");
                CheckIfCanEnterDoor(guard.gameObject);
            }
        }

    }
    public void CheckIfCanEnterDoor(GameObject guard)
    {
        // Check that there is nothing in front of the door
        Collider2D[] blockers;
        blockers = Physics2D.OverlapCircleAll(transform.position, 0.4f,
                                                                    LayerMask.GetMask("Draggable"));
        if (blockers.Length == 0)
        {
            AccessDoor(guard);
        }

        foreach (Collider2D c in blockers)
        {
            Debug.Log("Name: " + c.gameObject.name);

            if (c.gameObject.layer == LayerMask.NameToLayer("Draggable"))
            {
                // Something IS blocking the way
                Debug.Log("Something is BLOCKING the door");

                TryRemoveBarricade(c);
            }
            else
            {
                // The door is passable
                // ---> Go through
                AccessDoor(guard);
            }
        }
    }

    public void AccessDoor(GameObject guard)
    {
        // Play opening Animation
        animator.Play("Open");

        // Start
    }

    private void TryRemoveBarricade(Collider2D blocker)
    {
        // Damage and Spin the barricade
        blocker.GetComponent<Draggable>().TakeDamage(10f);

        // Kick it!
        Rigidbody2D rb = blocker.GetComponent<Rigidbody2D>();

        rb.AddForce(Vector2.up * Random.Range(1f, 2f) * rb.mass, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-7f, 7f), ForceMode2D.Impulse);

    }

    //IEnumerator TryToMoveGuardBehindDoor()  // Should be handled by the door?
    //{
    //    // Check if 

    //    //bool unspawned = false;
    //    //while (unspawned)
    //    //{
    //    //    yield return new WaitForSeconds(0.5f);
    //    //}
    //}
    #endregion
}
