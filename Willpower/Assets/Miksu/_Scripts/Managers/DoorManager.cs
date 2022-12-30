using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    #region PROPERTIES
    List<Door> doors = new List<Door>();

    GameObject player;
    float heightDifferenceTolerance = 5f;

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

        //Debug.Log("Number of doors: " + doors.Count);
        #endregion

        // Get Player
        player = FindObjectOfType<ProtagController>().gameObject;
    }
    #endregion

    #region SPAWNING

    #endregion


    #region Guards AI
    public GameObject GoToNearestDoor(Vector3 currentPosition)
    {
        //Debug.Log("Doors[0]: " + doors)
        //return doors[0].gameObject;

        float currentYpos = currentPosition.y;

        // Check if the only door
        if (doors.Count == 1)
        {
            return doors[0].gameObject;
        }

        List<Door> viableDoors = new List<Door>();


        foreach (Door door in doors)
        {
            // Check which doors are on the same Y level or above
            //                                                   --> Not below
            if (door.transform.position.y <= currentYpos + 1f)
            {
                viableDoors.Add(door);
            }
        }

        // If no viable doors -> All are out of reach
        if (viableDoors.Count == 0)
        {
            return null;
        }

        // Check which doors are closest to the selected Y level
        float highestYvalue = viableDoors[0].transform.position.y;
        foreach (Door door in viableDoors)
        {
            #region Old
            ////if (door.transform.position.y < closestDoor.transform.position.y)
            //if (door.transform.position.y < highestYvalue)
            //{
            //    // If smaller (further), remove from doors
            //    candidateDoors.Remove(door);
            //}
            //else
            //{
            //    // Is same or higher?
            //    //if (door.transform.position.y > closestDoor.transform.position.y)
            //    if (door.transform.position.y > highestYvalue)
            //    {
            //        // Challenger is higher, remove closest Door
            //        //candidateDoors.Remove(closestDoor);

            //        // New closest Door
            //        highestYvalue = door.transform.position.y;
            //    }
            //    // else
            //    // --> Keep them both
            //}
            #endregion

            if (door.transform.position.y > highestYvalue)
            {
                // Set the new gihgest y value
                highestYvalue = door.transform.position.y;
            }
        }

        //Debug.Log("Highest Y value: " + highestYvalue);

        List<Door> candidateDoors = new List<Door>();
        foreach (Door door in viableDoors)
        {
            // If one of the doors with highest Y value
            if (door.transform.position.y >= highestYvalue - 1f)
            {
                // add to the candidate list
                candidateDoors.Add(door);
            }
        }

        // Get one of those
        int random = Random.Range(0, candidateDoors.Count);

        return candidateDoors[random].gameObject;
    }

    #endregion

    #region Guard TRANSPORTING

    public void TransportGuardBehindAnotherDoor(GameObject guard)
    {

        // Get Player Y coord
        float playerY = player.transform.position.y;

        // Get all the doors that are on that level or higher
        List<Door> viableDoors = new List<Door>();

        foreach (Door door in doors)
        {
            if (door.transform.position.y >= playerY - heightDifferenceTolerance)
            {
                viableDoors.Add(door);
            }
        }

        // Transport Guard ownership to that Door
        if (viableDoors.Count >= 1)
        {
            viableDoors[Random.Range(0, viableDoors.Count)].TransportGuard(guard);
        }
        else
        {
            // Random door if no viable doors
            doors[Random.Range(0, doors.Count)].TransportGuard(guard);
        }
    }

    #endregion
}
