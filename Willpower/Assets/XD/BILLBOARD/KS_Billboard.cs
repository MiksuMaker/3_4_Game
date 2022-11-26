using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Billboard : MonoBehaviour
{

    public Camera cameraToLookAt;


    private void Awake()
    {
        cameraToLookAt = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {


        transform.LookAt(cameraToLookAt.transform);

    }
}
