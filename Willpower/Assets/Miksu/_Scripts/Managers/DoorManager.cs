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
        foreach (Door _door in _doors)
        {
            doors.Add(_door);
        }

        Debug.Log("Number of doors: " + doors.Count);
        #endregion

        // Get Player
        player = FindObjectOfType<ProtagController>().gameObject;
    }
    #endregion

    #region SPAWNING

    #endregion

    #region MOVING
    public GameObject GoToNearestDoor(Vector3 currentPosition)
    {
        //Debug.Log("Doors[0]: " + doors)
        return doors[0].gameObject;

        //float yCoord = currentPosition.y;

        //// Check if the only door
        //if (doors.Count == 1)
        //{
        //    return doors[0].gameObject;
        //}

        ////Door nearestDoor = doors[0];
        ////float nearest_Y_Level = doors[0].transform.position.y;


        //List<Door> viableDoors = new List<Door>();


        //foreach (Door door in doors)
        //{
        //    // Check which doors are on the same level or above

        //    if (door.transform.position.y <= yCoord)
        //    {
        //        viableDoors.Add(door);
        //    }
        //}

        //// Check which doors are closest on the Y level
        //Door closestDoor = viableDoors[0];
        //foreach (Door door in viableDoors)
        //{
        //    // If challenger is lower than the closest Door
        //    if (door.transform.position.y < closestDoor.transform.position.y)
        //    {
        //        // If smaller (further), remove from doors
        //        viableDoors.Remove(door);
        //    }
        //    else
        //    {
        //        // Is same or higher?
        //        if (door.transform.position.y == closestDoor.transform.position.y)
        //        {
        //            // Same, keep them both
        //        }
        //        else
        //        {
        //            // Challenger is higher, remove closest Door
        //            viableDoors.Remove(closestDoor);

        //            // New closest door
        //            closestDoor = door;
        //        }
        //    }
        //}

        //// Get one of those
        //return viableDoors[Random.Range(0, viableDoors.Count + 1)].gameObject;
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
