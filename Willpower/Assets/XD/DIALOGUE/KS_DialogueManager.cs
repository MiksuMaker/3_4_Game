using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KS_DialogueManager : MonoBehaviour
{

    KS_CutsceneHandler cutsceneHandler;

    public TextMeshProUGUI nametext;
    public TextMeshProUGUI dialogueText;
    public TMP_Text talkedCounterText;

    private KS_DialogueActivator dialAct;

    bool finished = false;


    public KS_DialogueTrigger[] talked_list;



    [SerializeField] private AudioSource audioSource;
    public AudioClip au1;

    public Animator animator; 

    private Queue<string> sentences;

    void Start()
    {
        dialAct = FindObjectOfType<KS_DialogueActivator>();
        sentences = new Queue<string>();

        talked_list = FindObjectsOfType<KS_DialogueTrigger>();

        cutsceneHandler = FindObjectOfType<KS_CutsceneHandler>();
        

    }

    public void StartDialogue(KS_Dialogue _dial)
    {

        animator.SetBool("IsOpen", true);



        nametext.text = _dial.name;

        sentences.Clear();

        foreach (string sentence in _dial.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        StartCoroutine(soundPlay());

    }


    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            
            dialogueText.text += letter;
            yield return new WaitForSeconds((float)0.025);
        }
        StopAllCoroutines();
    }


    IEnumerator soundPlay()
    {
        yield return new WaitForSeconds((float)0.1);
        audioSource.pitch = Random.Range((float)0.9, (float)1.1);
        audioSource.PlayOneShot(au1);
        StartCoroutine(soundPlay());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        animator.SetBool("IsOpen", false);
        dialAct.isTalking = false;

        if (!finished)
        {
            if (checkTalkedList()){
                dialAct.setCantalk(false);
                cutsceneHandler.setPhase("MID");
            }
        }
    }

    public bool checkTalkedList()
    {
        UpdateTalkedCounter();
        foreach (KS_DialogueTrigger _d in talked_list)
        {
            if (!_d.getTalked()) { return false; }
        }
        finished = true;
        return true;
    }

    public void UpdateTalkedCounter()
    {
        int counter = 0;
        foreach (KS_DialogueTrigger _d in talked_list)
        {
            if (_d.getTalked()) { counter++; }
        }
        counter--;
        talkedCounterText.text = $"{counter}/9";
    }

    public bool getFinished()
    {
        return finished;
    }

    public void EnableTalkedCounter()
    {
        talkedCounterText.gameObject.SetActive(true);
    }

    public void DisableTalkedCounter()
    {
        talkedCounterText.gameObject.SetActive(false);
    }
}
