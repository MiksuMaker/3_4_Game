using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WillScript : MonoBehaviour
{
    // Start is called before the first frame update
    private string lastPressed;

    public static string whoGetsHouse = "";
    public static string whoGetsMoney = "";
    public static string  whoGetsCompany = "";
    public static string whoGetsDog = "";

    [SerializeField] GameObject charMenu;
    [SerializeField] GameObject sign;
    [SerializeField] GameObject end;

    #region Choicebox opening
    public void TakePlace(GameObject button)
    {
        lastPressed = button.name;

        charMenu.transform.position = new Vector3(charMenu.transform.position.x, button.transform.position.y, charMenu.transform.position.z);
        charMenu.SetActive(true);
    }

    //public void

    #endregion Choicebox opening

    public void ChooseWho(string who)
    {
        switch (lastPressed){ 
            case "House":
                whoGetsHouse = who;
                break;
            case "Money":
                whoGetsMoney = who;
                break;
            case "Company":
                whoGetsCompany = who;
                break;
            case "Dog":
                whoGetsDog = who;
                break;
            default:
                Debug.Log("Nobody can't inherit" +lastPressed);
                break;
        } //edit who inherits the thing.

        GameObject.Find(lastPressed).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = who;
        charMenu.SetActive(false);
    } //Choose who is the inheritor.

    public void Sign()
    {
        if(whoGetsMoney != "" && whoGetsCompany != "" && whoGetsDog != "" && whoGetsHouse != "")
        {
            sign.SetActive(true);
            StartCoroutine(WaitAfterSign());
        }

    }

    IEnumerator WaitAfterSign()
    {
        yield return new WaitForSeconds(2f);
        end.SetActive(true);
        gameObject.SetActive(false);
    }
}
