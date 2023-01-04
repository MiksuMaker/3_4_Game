using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Draggable
{
    [SerializeField] float bulletSpeed = 5f;

    //[SerializeField] LayerMask[] hitLayers;


    protected override void FixedUpdate()
    {
        FlyForwards();
    }

    public override void DragMeAround(Vector2 mousePos, float dragForce)
    {
        base.DragMeAround(mousePos, dragForce * 0.01f);
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

        if (hurtIndicator != null)
        {
            GameObject hurt = Instantiate(hurtIndicator, transform.position, transform.rotation) as GameObject;
            hurt.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Destroy(hurt, 1f);
        }
        // Destroy
        Destroy(gameObject);
    }
}
