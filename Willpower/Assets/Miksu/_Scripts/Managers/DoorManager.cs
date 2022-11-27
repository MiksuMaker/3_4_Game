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
    public void FindNearestDoor(float yCoord)
    {
        Door nearestDoor = doors[0];

        foreach (Door door in doors)
        {

        }
    }
    #endregion
}
