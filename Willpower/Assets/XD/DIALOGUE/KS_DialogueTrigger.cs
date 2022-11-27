using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_DialogueTrigger : MonoBehaviour
{
    public KS_Dialogue dial;

    private KS_DialogueManager dialMana;



    [SerializeField] bool talked = false;

    private void Awake()
    {

        dialMana = FindObjectOfType<KS_DialogueManager>();

        //_id = dialMana.talked_list.size

    }


    public void TriggerDialogue()
    {
        talked = true;
        dialMana.StartDialogue(dial);
    }

    public void ContinueDialogue()
    {
        dialMana.DisplayNextSentence();
    }

    public bool getTalked()
    {
        return talked;
    }
}
