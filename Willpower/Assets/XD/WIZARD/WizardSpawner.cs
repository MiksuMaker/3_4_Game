using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSpawner : MonoBehaviour
{
    public GameObject[] wizards;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWizards());
    }

    IEnumerator SpawnWizards()
    {
        foreach (GameObject wizard in wizards)
        {
            yield return new WaitForSeconds(0.5f);
            wizard.gameObject.SetActive(true);
        }
        
    }
}
