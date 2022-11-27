using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_CutsceneHandler : MonoBehaviour
{

    KS_DialogueManager dialMana;

    string phase = "START";
    int phase_alt = 0;
    bool con = true;

    [SerializeField] private KS_Dialogue cut_beginning;

    [SerializeField] private KS_Dialogue cut_middle;

    [SerializeField] private KS_Dialogue cut_end;


    private void Start()
    {
        dialMana = FindObjectOfType<KS_DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!con) { return; }
        switch (phase)
        {

            case "START":
               
                switch (phase_alt)
                {
                    case 0 : wait(1);phase_alt++; break;
                    case 1: dialMana.StartDialogue(cut_beginning); phase_alt++; break;
                    case 2: case 3: case 4: case 5: if (click()) { dial_c();phase_alt++; };break;


                }
                break;


        }

    }


    IEnumerator wait_co(float time)
    {
        yield return new WaitForSeconds(time);
        con = true;
    }


    bool click()
    {
        return Input.GetMouseButtonDown(0);
    }

    void dial_c()
    {
        dialMana.DisplayNextSentence();
    }

    void wait(float time)
    {
        con = false;
        StartCoroutine(wait_co(time));
    }
}
