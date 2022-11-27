using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndS : MonoBehaviour
{

    [SerializeField] GameObject picBox;
    [SerializeField] GameObject textBox;

    private void OnEnable()
    {
        StartCoroutine(GoThroughEnds());
    }

    IEnumerator GoThroughEnds()
    {
        TextMeshProUGUI textSays = textBox.GetComponent<TextMeshProUGUI>();
        Image pic = picBox.GetComponent<Image>();

        yield return new WaitForSeconds(1f);

        switch (WillScript.whoGetsHouse)
        {
            case "Taenia":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Willow":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Mortti":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Father":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Doge":
                textSays.text = "";
                pic.sprite = null;
                break;
        }

        switch (WillScript.whoGetsMoney)
        {
            case "Taenia":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Willow":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Mortti":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Father":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Doge":
                textSays.text = "";
                pic.sprite = null;
                break;
        }

        switch (WillScript.whoGetsCompany)
        {
            case "Taenia":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Willow":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Mortti":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Father":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Doge":
                textSays.text = "";
                pic.sprite = null;
                break;
        }

        switch (WillScript.whoGetsDog)
        {
            case "Taenia":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Willow":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Mortti":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Father":
                textSays.text = "";
                pic.sprite = null;
                break;
            case "Doge":
                textSays.text = "";
                pic.sprite = null;
                break;
        }

    }
}
