using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_MakeBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
                bullet.transform.rotation = this.transform.rotation;
                KS_Bullet bul = bullet.GetComponent<KS_Bullet>();
                bul.ResetStuff();
                bullet.SetActive(true);
                bul.StartKillTimer();
            }
        }
    }
}
