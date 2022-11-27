using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerMK : MonoBehaviour
{
    public enum Character { Taenia, Willow, Mortti, Father, Doge }; //enum for character.
    public static Character charNow;

    public static int qLeft = 10;

    [SerializeField] GameObject conversation;


    #region Open/close conversation
    public void openConversation()
    {
        conversation.SetActive(true);
    }

    public void closeConversation()
    {
        GameObject conversation = GameObject.Find("Conversation");
        conversation.SetActive(false);
    }

    #endregion


    public static void OpenWill()
    {

    }
}
