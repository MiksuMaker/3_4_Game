using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KS_Billboard : MonoBehaviour
{

    public Camera cameraToLookAt;
    public bool rotateX = false;


    private void Awake()
    {
        cameraToLookAt = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        


        transform.LookAt(cameraToLookAt.transform);

        if (!rotateX)
        {
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y+180, transform.rotation.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z);
        }
    }
}
