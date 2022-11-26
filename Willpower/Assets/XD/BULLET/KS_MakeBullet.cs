using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_MakeBullet : MonoBehaviour
{
    public Camera cameraToLookAt;
    // Start is called before the first frame update
    void Start()
    {
        cameraToLookAt = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = KS_ObjectPooler.KS_SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = this.transform.position;
                bullet.transform.rotation = cameraToLookAt.transform.rotation;
                KS_Bullet bul = bullet.GetComponent<KS_Bullet>();
                bul.ResetStuff();
                bullet.SetActive(true);
                bul.StartKillTimer();
            }
        }
    }
}
