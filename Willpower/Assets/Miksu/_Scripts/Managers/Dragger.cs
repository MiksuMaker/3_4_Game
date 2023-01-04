using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    // This class is responsible for handling the DRAGGING
    // of every DRAGGABLE object

    #region PROPERTIES
    // Delegate
    public delegate void ToggleDrag(bool onOff);
    public delegate ToggleDrag ToggleGlobalDrag(bool onOff);
    //public delegate bool CheckDragList(Draggable draggable);
    //public delegate CheckDragList CheckIfOnList(Draggable draggable);

    // References
    Camera camera;
    BoxCollider2D boxCollider;
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
    int[] directions = {-1, 1};
    #endregion

    #region BUILTIN
    private void Start()
    {
        camera = Camera.main;
        boxCollider = GetComponent<BoxCollider2D>();
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

    private void ToggleDragging(bool onOff)
    {
        // Switch Box collider on or off
        boxCollider.enabled = onOff;
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
            Draggable obj = collider.GetComponent<Draggable>();
            obj.InitializeDrag();
            objectsInPull.Add(obj);
        }

        // Add random torque
        foreach (Draggable d in objectsInPull)
        {
            if (d is not Bullet)
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
            if (obj != null)
            {
                obj.DragMeAround(mousePos, grabForce * 100f * Time.deltaTime);
            }
        }
    }

    // TIME.DELTATIME TESTING ZONE

    // Move along!

    //
    //private float deleteThis = 1f;
    //private float time;
    //private int frameCount = 0;
    //private void Update()
    //{
    //    Debug.Log("Deltatime: " + Time.deltaTime);
    //    //time += Time.deltaTime;
    //    //frameCount++;

    //    //if (time >= deleteThis)
    //    //{
    //    //    int frameRate = Mathf.RoundToInt(frameCount / time);
    //    //    Debug.Log("Framerate: " + frameRate);
    //    //    frameCount = 0;
    //    //    time = 0f;
    //    //}
    //}

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
            draggable.rb.AddTorque(GiveRandomDirection(), ForceMode2D.Impulse);
        }
    }

    private float GiveRandomDirection()
    {
        int dir = directions[Random.Range(0, 2)];

        // Return power torque
        return (dir * Random.Range(3f, 5f));
    }

    public void UndragMe(Draggable draggable)
    {
        // Remove from list
        objectsInPull.Remove(draggable);

        //foreach (Draggable obj in objectsInPull)
        //{
        //    if (obj == draggable)
        //    {
        //        // Remove from list
        //        break;
        //    }
        //}
    }
}
