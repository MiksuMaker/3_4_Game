using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    #region PROPERTIES
    // References
    [SerializeField] GameObject gun;
    Animator animator;
    [SerializeField] GameObject shootPoint;

    [SerializeField] GameObject bullet;

    GameObject player;

    SecurityController guard;
    #endregion

    private void Start()
    {
        player = FindObjectOfType<ProtagController>().gameObject;

        guard = transform.parent.GetComponent<SecurityController>(); if (!guard) { Debug.Log("No Guard found"); }
        animator = gun.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(Shooter());
    }

    private void Update()
    {
        // Check that the guard isn't dragged around
        if (guard.currentMode == SecurityController.aiMode.struggle) { return; }

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

    IEnumerator Shooter()
    {
        float wait = 0.5f;
        WaitForSeconds tick = new WaitForSeconds(wait);
        float shootTreshold = 2f;
        float currentPressure = 0f;

        while (true)
        {
            yield return tick;

            if (CheckForLineOfSight() && guard.currentMode != SecurityController.aiMode.struggle)
            {
                currentPressure += wait;

                // Check if time to shoot
                if (currentPressure >= shootTreshold)
                {
                    // Shoot!
                    Shoot();

                    // Reset trigger
                    currentPressure = 0f;
                }
            }
            else
            {
                if (currentPressure > 0f)
                {
                    currentPressure -= wait;
                }
            }
        }
    }

    private void Shoot()
    {
        animator.Play("GunShoot");

        if (bullet != null)
        {
            //Quaternion rot = new Quaternion(transform.rotation.x, transform.rotation.y, -transform.rotation.z, 1f);
            //Quaternion rot = Quaternion.Inverse(gun.transform.rotation);
            //Vector3 opposite = -gun.transform.right;
            //Quaternion rot = Quaternion.Euler(opposite);
            Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation);
        }
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
            //Debug.Log("Orientation: " + guard.currentOrientation);
            targetRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);
    }

    private bool CheckForLineOfSight()
    {
        // Check if anything is between the Guard and Player

        Vector2 line = PPos() - transform.position;
        Vector2 dir = line.normalized;
        float dist = line.magnitude;
        if (!Physics2D.Raycast(transform.position, dir, dist, LayerMask.GetMask("Default")))
        {
            // If no, line of sight is achieved
            Debug.DrawLine(transform.position, PPos(), Color.red);
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
        float angle = Mathf.Atan2(transform.position.y - PPos().y,
                                   -transform.position.y - PPos().x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500f * Time.deltaTime);
    }

    private Vector3 PPos()
    {
        // Get player position with added y coord so that the aim is at center of Player
        Vector3 desiredAimPos = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f);

        return desiredAimPos;
    }

}
