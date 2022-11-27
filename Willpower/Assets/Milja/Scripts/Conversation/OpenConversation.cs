using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenConversation : MonoBehaviour
{
    [SerializeField] GameManagerMK.Character thisChar;

    public void OnMouseDown()
    {
        GameManagerMK.charNow = thisChar;
        GameObject.Find("GameManager").GetComponent<GameManagerMK>().openConversation();
    }
}
