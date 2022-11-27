using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSpawner : MonoBehaviour
{
    public GameObject[] wizards;

    public int wizCur = 0;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnWizards());
    }

    public IEnumerator SpawnWizards()
    {
        foreach (GameObject wizard in wizards)
        {
            wizCur++;
            yield return new WaitForSeconds(0.5f);
            wizard.gameObject.SetActive(true);
        }
        
    }


    public bool areAllWizardsLikePwnedOrSomething()
    {
        return wizCur <= 0;
    }

}
