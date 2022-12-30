using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] int floor = 0;
    float height;

    DoorManager doorManager;
    public List<GameObject> guardsBehindDoor = new List<GameObject>();
    bool guardExitInProgress = false;

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
        doorManager = FindObjectOfType<DoorManager>();
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
                //Debug.Log("Guard trying to enter");
                CheckIfCanEnterDoor(guard.gameObject);
            }
        }

    }

    #region DOOR ENTER/EXIT LEGIBILITY
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
            //Debug.Log("Name: " + c.gameObject.name);

            if (c.gameObject.layer == LayerMask.NameToLayer("Draggable"))
            {
                // Something IS blocking the way
                //Debug.Log("Something is BLOCKING the door");

                TryRemoveBarricade(c);
            }
        }
    }

    public bool CheckIfCanExitDoor()
    {
        // Check that there is nothing in front of the door
        Collider2D[] blockers;
        blockers = Physics2D.OverlapCircleAll(transform.position, 0.4f,
                                                                    LayerMask.GetMask("Draggable"));
        if (blockers.Length == 0)
        {
            return true;
        }

        foreach (Collider2D c in blockers)
        {

            if (c.gameObject.layer == LayerMask.NameToLayer("Draggable"))
            {
                // Something IS blocking the way
                //Debug.Log("Something is BLOCKING the door");

                TryRemoveBarricade(c);
            }
        }
        return false;
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

    #endregion

    public void AccessDoor(GameObject guard)
    {
        // Play opening Animation
        animator.Play("Open");

        // Start

        // Transform Guard Ownership to another door
        doorManager.TransportGuardBehindAnotherDoor(guard);
    }

    public void TransportGuard(GameObject guard)
    {
        // Deactivate Guard
        guard.SetActive(false);

        // Add the guard to the wait list
        guardsBehindDoor.Add(guard);

        // Move Guard to this door
        guard.transform.position = transform.position;

        // Begin the Exit process
        if (!guardExitInProgress)
        {
            StartCoroutine(GuardExitCoroutine());
        }
    }

    private void ExitGuard()
    {

        // Play animation
        animator.Play("Open");

        // Choose a guard from the list
        GameObject guard = guardsBehindDoor[Random.Range(0, guardsBehindDoor.Count)];

        // Reactivate Guard
        guard.SetActive(true);

        // Remove them from the list
        guardsBehindDoor.Remove(guard);

    }

    IEnumerator GuardExitCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        guardExitInProgress = true;

        yield return wait;
        while (guardsBehindDoor.Count != 0)
        {

            // Check if the door is blocked
            if (CheckIfCanExitDoor())
            {

                // Get out
                ExitGuard();

            }

            yield return wait;
        }

        guardExitInProgress = false;
    }


    #endregion
}
