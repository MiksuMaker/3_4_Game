using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIscript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject items, help;

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            items.SetActive(true);
            help.SetActive(false);
        }
    }

    public void Skip()
    {
        GameManagerMK.qLeft = 0;
    }
}
