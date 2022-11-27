using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerMK : MonoBehaviour
{
    public enum Character { Taenia, Willow, Mortti, Father, Doge }; //enum for character.
    public static Character charNow;

    public static int qLeft = 10;

    [SerializeField] GameObject conversation;
    [SerializeField] GameObject will;

    #region Open/close conversation
    public void openConversation()
    {
        conversation.SetActive(true);
    }

    public void closeConversation()
    {
        conversation.SetActive(false);
    }

    #endregion

    public void Update()
    {
        if(qLeft < 1)
        {
            GameObject.Find("Room").SetActive(false);
            OpenWill();
            qLeft = 100;
            GameObject.Find("TextCounter").GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void OpenWill()
    {

        closeConversation();
        will.SetActive(true);

    }
}
