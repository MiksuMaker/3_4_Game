using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WizardSpawner : MonoBehaviour
{
    public GameObject[] wizards;
    [SerializeField] GameObject WizardCounter;
    [SerializeField] TMP_Text wizardCountText;
    [SerializeField] Animator textAnimator;

    public int wizCur = 0;
    public int defeatedWiz = 0;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnWizards());
    }

    public IEnumerator SpawnWizards()
    {
        WizardCounter.SetActive(true);
        foreach (GameObject wizard in wizards)
        {
            wizCur++;
            wizardCountText.text = $"{defeatedWiz}/{wizards.Length}";
            yield return new WaitForSeconds(0.5f);
            wizard.gameObject.SetActive(true);
        }
        
    }

    public void UpdateText(int num)
    {
        defeatedWiz += num;
        if (defeatedWiz >= wizards.Length)
        {
            //textAnimator.SetBool("FadeOut", true);
            StartCoroutine(DisableText());
        }
        wizardCountText.text = $"{defeatedWiz}/{wizards.Length}";
    }


    public bool areAllWizardsLikePwnedOrSomething()
    {
        return wizCur <= 0;
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(3f);
        WizardCounter.SetActive(false);
    }
}
