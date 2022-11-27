using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertoTo2D : MonoBehaviour
{
    [SerializeField] GameObject wizardParentObject;
    [SerializeField] Animator wizardAnimator;
    [SerializeField] SkinnedMeshRenderer SkinnedMeshRenderer;
    [SerializeField] GameObject wizard_2d;

    public bool convertTo2D = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (convertTo2D) ConvertTo2DSprite();
    }

    public void ConvertTo2DSprite()
    {
            SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer.enabled = false;
            wizard_2d.SetActive(true);
            StartCoroutine(KillWizard());
    }

    IEnumerator KillWizard()
    {
        yield return new WaitForSeconds(2f);
        wizardAnimator.SetBool("Kill", true);
        yield return new WaitForSeconds(2f);
        Destroy(wizardParentObject);
    }
}
