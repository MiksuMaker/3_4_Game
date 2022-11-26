using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_DialogueActivator : MonoBehaviour
{

    bool clicked = false;
    public bool isTalking = false;

    Collider colli;

    KS_DialogueManager dialMana;
    KS_DialogueTrigger dial;


    private void Start()
    {
        dialMana = FindObjectOfType<KS_DialogueManager>();
    }

    private void Update()
    {
        clicked = Input.GetMouseButtonDown(0);   
        if (clicked){
            if (dial != null)
            {

                if (!isTalking)
                {
                    isTalking = true;
                    dial.TriggerDialogue();
                }
                else
                {
                    dial.ContinueDialogue();
                }
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        //take the dialogue component from the trigget object
        dial = other.gameObject.GetComponent<KS_DialogueTrigger>();
    }

    private void OnTriggerExit(Collider other)
    {
        dialMana.EndDialogue();
        if (dial == other.gameObject.GetComponent<KS_DialogueTrigger>())
        {
            dial = null;
        }
    }
}
