using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenConversation : MonoBehaviour
{
    [SerializeField] GameManager.Character thisChar;

    public void OnMouseDown()
    {
        GameManager.charNow = thisChar;
        GameObject.Find("GameManager").GetComponent<GameManager>().openConversation();
    }
}
