using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 5f;

    //[SerializeField] LayerMask[] hitLayers;


    private void FixedUpdate()
    {
        FlyForwards();
    }

    private void FlyForwards()
    {
        // Move Forwards
        transform.position = (transform.position + transform.right * bulletSpeed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        //{
        //    //Debug.Log("Hit Layer " + collision.gameObject.layer.ToString());

        //    // Hit
        //}

        // Destroy
        Destroy(gameObject);
    }
}
