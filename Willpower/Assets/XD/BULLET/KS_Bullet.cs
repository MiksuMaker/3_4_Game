using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Bullet : MonoBehaviour
{

    public float movementSpeed = 1;

    Vector3 rot;
    Vector3 forw;

    GameObject playerObject = null;

    private void Awake()
    {
    
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += forw * Time.deltaTime * movementSpeed;
    }


    public void ResetStuff()
    {
        if (playerObject != null)
        {
         
                
            rot = playerObject.transform.rotation.eulerAngles.normalized;
                forw = playerObject.transform.forward;
                
           
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wizard")) gameObject.SetActive(false);
        else return;
    }

    public void StartKillTimer()
    {
        StopAllCoroutines();
        StartCoroutine(killMePlz());
    }

    IEnumerator killMePlz()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

    public void SetPlayer(GameObject p)
    { 
        playerObject = p;
    }

}
