using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    #region PROPERTIES
    // References
    [SerializeField] GameObject gun;
    [SerializeField] GameObject shootPoint;

    [SerializeField] GameObject bullet;

    GameObject player;

    SecurityController guard;
    #endregion

    private void Start()
    {
        player = FindObjectOfType<ProtagController>().gameObject;

        guard = transform.parent.GetComponent<SecurityController>(); if (!guard) { Debug.Log("No Guard found"); }

    }

    private void Update()
    {
        if (CheckForLineOfSight())
        {
            AimTowardsPlayer();

        }
        else
        {
            KeepWeaponForward();
        }

        CheckWeaponOrientation();

    }

    private void KeepWeaponForward()
    {
        Quaternion targetRotation;

        if (guard.currentOrientation == SecurityController.Orientation.left)
        {
            // Point the weapon left
            targetRotation = Quaternion.Euler(Vector2.left);
            //targetRotation = Quaternion.Euler(Vector2.right);
        }
        else
        {
            // Point it right
            Debug.Log("Orientation: " + guard.currentOrientation);
            targetRotation = Quaternion.Euler(new Vector3(0,0, 180));
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);
    }

    private bool CheckForLineOfSight()
    {
        // Check if anything is between the Guard and Player

        Vector2 line = player.transform.position - transform.position;
        Vector2 dir = line.normalized;
        float dist = line.magnitude;
        if (!Physics2D.Raycast(transform.position, dir, dist, LayerMask.GetMask("Default")))
        {
            // If no, line of sight is achieved
            Debug.DrawLine(transform.position, player.transform.position, Color.red);
            return true;
        }

        return false;
    }

    private void CheckWeaponOrientation()
    {
        if (gun.transform.position.x < transform.position.x)
        {
            // Looking left
            gun.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // Looking right
            gun.transform.localScale = new Vector3(1, -1, 1);
        }
    }

    private void AimTowardsPlayer()
    {
        // Point the pistol towards the Player
        float angle = Mathf.Atan2(transform.position.y - player.transform.position.y,
                                   -transform.position.y - player.transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);
    }

}
