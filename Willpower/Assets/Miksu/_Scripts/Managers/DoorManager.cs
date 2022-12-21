using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    #region PROPERTIES
    List<Door> doors = new List<Door>();

    GameObject player;
    #endregion

    #region SETUP
    private void Start()
    {
        // Get the doors
        #region door fetcing
        Door[] _doors = FindObjectsOfType<Door>();
        foreach (Door _door in doors)
        {
            doors.Add(_door);
        }
        #endregion

        // Get Player
        player = FindObjectOfType<ProtagController>().gameObject;
    }
    #endregion

    #region SPAWNING

    #endregion

    #region MOVING
    public Vector3 FindNearestDoor(float yCoord)
    {
        Door nearestDoor = doors[0];
        float nearest_Y_Level = doors[0].transform.position.y;

        foreach (Door door in doors)
        {
            // Check if on the same level
            //if (door.transform)

            // Check which door is closest
        }

        return Vector3.zero;
    }
    #endregion
}
