using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIscript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject items, help, conversation;
    bool convActive = true;

    public void Update()
    {
        if (Input.anyKeyDown && convActive)
        {
            items.SetActive(true);
            help.SetActive(false);
            conversation.SetActive(false);
            convActive = false;
        }
    }

    public void Skip()
    {
        GameManagerMK.qLeft = 0;
    }
}
