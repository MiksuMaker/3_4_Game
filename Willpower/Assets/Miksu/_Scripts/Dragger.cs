using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    // This class is responsible for handling the DRAGGING
    // of every DRAGGABLE object

    #region PROPERTIES
    // Delegate
    //public delegate bool CheckDragList(Draggable draggable);
    //public delegate CheckDragList CheckIfOnList(Draggable draggable);

    // References
    Camera camera;
    [SerializeField] LayerMask targetMask;
    Collider2D[] collidersInPull;
    List<Draggable> objectsInPull = new List<Draggable>();

    [Header("DEBUG")]
    public bool DEBUG_On = true;
    public Vector2 clickPos = Vector2.zero;
    public Vector2 mousePos = Vector2.zero;

    [Header("Grabbing")]
    [SerializeField]
    float grabRadius = 2f;

    [Header("Physics")]
    [SerializeField]
    float grabForce = 50f;
    #endregion

    #region BUILTIN
    private void Start()
    {
        camera = Camera.main;
    }

    private void OnDrawGizmos()
    {
        if (DEBUG_On)
        {
            // Draw a circle around the click pos
            Gizmos.DrawWireSphere(clickPos, grabRadius);

            // Draw a circle around MOUSE pos
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(mousePos, grabRadius);
        }
    }
    #endregion

    #region DRAGGING

    // When Mouse is CLICKED, Grab nearby Draggable objects
    private void OnMouseDown()
    {
        // Get mouse Pos
        clickPos = GetMousePos();

        // Clear lists
        collidersInPull = null;
        objectsInPull.Clear();

        // Collect all the things within
        collidersInPull = Physics2D.OverlapCircleAll(clickPos, grabRadius, targetMask);

        // Get reference to the Dragger scripts
        foreach (Collider2D collider in collidersInPull)
        {
            objectsInPull.Add(collider.GetComponent<Draggable>());
        }

        // Add random torque
        foreach (Draggable d in objectsInPull)
        {
            AddRandomTorque(d);
        }
    }

    // When Mouse is DRAGGED, drag the nearby Draggables towards mouse
    private void OnMouseDrag()
    {
        // Update mousePos
        mousePos = GetMousePos();

        // Drag the things
        foreach (Draggable obj in objectsInPull)
        {
            obj.DragMeAround(mousePos, grabForce);
        }
    }

    private Vector2 GetMousePos()
    {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool CheckObjectsInPullList(Draggable draggable)
    {
        // Check if the draggable is on the objectsInPull list

        foreach (Draggable obj in objectsInPull)
        {
            if (obj == draggable)
            {
                return true;
            }
        }

        // Else
        return false;
    }
    #endregion

    #region CALCULATIONS

    #endregion

    private void AddRandomTorque(Draggable draggable)
    {
        // Check that the Object has no velocity
        //if (draggable.rb.velocity.magnitude < 0.1f)
        if (draggable.rb.angularVelocity < 0.5f)
        {
            draggable.rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);
        }
    }
}
