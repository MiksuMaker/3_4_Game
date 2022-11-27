using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_DialogueActivator : MonoBehaviour
{


    bool canTalk = true;
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
        if (!canTalk) { return; }
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
        if (!canTalk) { return; }
        //take the dialogue component from the trigget object
        dial = other.gameObject.GetComponent<KS_DialogueTrigger>();
    }

    private void OnTriggerExit(Collider other)
    {

        if (!canTalk) { return; }
        dialMana.EndDialogue();
        if (dial == other.gameObject.GetComponent<KS_DialogueTrigger>())
        {
            dial = null;
        }
    }

    public void setCantalk(bool _canTalk)
    {
        canTalk = _canTalk;
    }
}
