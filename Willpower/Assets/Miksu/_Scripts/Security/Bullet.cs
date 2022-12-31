using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 5f;

    [SerializeField] LayerMask[] hitLayers;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        FlyForwards();
    }

    private void FlyForwards()
    {
        // Move Forwards
        transform.position = (transform.position + transform.right * bulletSpeed * Time.deltaTime);

        // Check for collisions
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        Debug.Log("Finish me!!!");
        //if (Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.LayerToName(hitLayers)))
    }
}
