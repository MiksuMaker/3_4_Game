using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_DialogueTrigger : MonoBehaviour
{
    public KS_Dialogue dial;

    private KS_DialogueManager dialMana;


    private void Awake()
    {
        dialMana = FindObjectOfType<KS_DialogueManager>();
    }


    public void TriggerDialogue()
    {
        dialMana.StartDialogue(dial);
    }

    public void ContinueDialogue()
    {
        dialMana.DisplayNextSentence();
    }
}
