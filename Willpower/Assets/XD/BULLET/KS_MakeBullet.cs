using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_MakeBullet : MonoBehaviour
{
    public Camera cameraToLookAt;
    public Sprite[] letters;
    private int index = 0;

    [SerializeField] Transform bulletSpawnPoint;

    [SerializeField] PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        cameraToLookAt = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.CanShoot)
        {
            GameObject bullet = KS_ObjectPooler.KS_SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.GetComponent<SpriteRenderer>().sprite = letters[index];
                bullet.transform.position = bulletSpawnPoint.transform.position;
                bullet.transform.rotation = cameraToLookAt.transform.rotation;
                KS_Bullet bul = bullet.GetComponent<KS_Bullet>();
                bul.ResetStuff();
                bullet.SetActive(true);
                bul.StartKillTimer();
                if (index == letters.Length - 1) index = 0;
                else index++;
            }
        }
    }
}
