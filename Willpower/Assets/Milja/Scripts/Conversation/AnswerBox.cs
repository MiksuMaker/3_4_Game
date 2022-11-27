using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnswerBox : MonoBehaviour
{   
    [SerializeField] GameObject reply;
    [SerializeField] GameObject charName, charSprite;

    private Object[] sprites;

    public void OnEnable()
    {

        sprites = Resources.LoadAll("MiljaSprites", typeof(Sprite));
        TextMeshProUGUI replyText = reply.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = charName.GetComponent<TextMeshProUGUI>();

        switch (GameManagerMK.charNow)
        {
            case GameManagerMK.Character.Taenia:
                nameText.text = "Taenia the Wife";
                replyText.text = "How can this happen? You were just fine before the lunch...";
                charSprite.GetComponent<Image>().sprite = FindSpriteWithName("wife");
                break;
            case GameManagerMK.Character.Willow:
                nameText.text = "Willow the Kid";
                replyText.text = "...";
                charSprite.GetComponent<Image>().sprite = FindSpriteWithName("willow");
                break;
            case GameManagerMK.Character.Mortti:
                nameText.text = "Mortti";
                replyText.text = "Good thing I managed to get here in time!";
                charSprite.GetComponent<Image>().sprite = FindSpriteWithName("mortti");
                break;
            case GameManagerMK.Character.Father:
                nameText.text = "Father";
                replyText.text = "Don't you stare at me.";
                charSprite.GetComponent<Image>().sprite = FindSpriteWithName("father");
                break;
            case GameManagerMK.Character.Doge:
                nameText.text = "Doge";
                replyText.text = "Arf?";
                charSprite.GetComponent<Image>().sprite = FindSpriteWithName("doge");
                break;
            default:
                Debug.Log("There is no character like that.");
                break;
        } //Change questions according to the character
    }

    Sprite FindSpriteWithName(string name)
    {
        foreach(Object i in sprites)
        {
            if(i.name == name)
            {
                return (Sprite)i;
            }
        }
        Debug.Log("There is no sprite named "+name);
        return null;
    }

    public void EditAnswer(string answer)
    {
        reply.GetComponent<TextMeshProUGUI>().text=answer;
    }
}
