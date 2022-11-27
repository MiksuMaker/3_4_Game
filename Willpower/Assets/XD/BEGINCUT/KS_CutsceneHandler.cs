using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_CutsceneHandler : MonoBehaviour
{

    KS_DialogueManager dialMana;
    KS_Music musicPlayer;
    [SerializeField] Animator willAnimator;
    [SerializeField] Animator startBgAnimator;

    [SerializeField] PlayerMovement player;

    
    [SerializeField] private GameObject image_start;


    [SerializeField] GameObject midSprite1;
    [SerializeField] GameObject midSprite2;
    [SerializeField] GameObject endSprite;

    [SerializeField] Animator tutoAnima;

    private KS_Billboard[] letsRemoveTheseShallWe;

    string phase = "START";
    int phase_alt = 0;
    bool con = true;

    [SerializeField] private KS_Dialogue cut_beginning;

    [SerializeField] private KS_Dialogue cut_middle;

    [SerializeField] private KS_Dialogue cut_end;

    [SerializeField] private WizardSpawner wizSpa;


    private void Start()
    {
        dialMana = FindObjectOfType<KS_DialogueManager>();
        musicPlayer = FindObjectOfType<KS_Music>();
        midSprite1.SetActive(false);
        midSprite2.SetActive(false);
        endSprite.SetActive(false);

        letsRemoveTheseShallWe = FindObjectsOfType<KS_Billboard>();

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
                    case 2: case 5:   if (click()) { dial_c();phase_alt++; };break;
                    case 3: if (click()) { willAnimator.SetBool("isAnim",true); phase_alt++; wait(2); } ;break;
                    case 4: { dial_c(); phase_alt++; }; break;
                    case 6: if (click()) { willAnimator.SetBool("isAnim", false); dialMana.EndDialogue() ; wait(2); phase_alt++; }; break;
                    case 7: startBgAnimator.SetBool("FADEOUT", true);phase_alt++; player.CanMove = true;phase_alt = 0;phase = "MID_WAIT";
                        tutoAnima.SetTrigger("KS_TutoAnima");
                        break;



                }
                break;


            case "MID":
                switch(phase_alt)
                {

                    case 0: wait(1); phase_alt++; player.CanMove = false; break;
                    case 1: startBgAnimator.SetBool("FADEOUT", false); phase_alt++;break;
                    case 2: dialMana.StartDialogue(cut_middle); phase_alt++; break;
                    case 5: dialMana.name = ""; phase_alt++;break;
                    case 8: dialMana.name = "wizard gäng";musicPlayer.musicStop();phase_alt++;
                        midSprite1.SetActive(true);
                        midSprite2.SetActive(true);

                        foreach (KS_Billboard billi in letsRemoveTheseShallWe)
                        {
                            billi.gameObject.SetActive(false);
                        }

                        player.gameObject.transform.position = wizSpa.gameObject.transform.position;
                        break;
                    case 11: ;phase_alt++;break;

                    case 13:
    
                        if (click()) {
                            midSprite1.SetActive(false);
                            midSprite2.SetActive(false);
                            dial_c(); phase_alt++; };
                        break;


                    case 15: startBgAnimator.SetBool("XDIMENSIO", true);
                        player.CanMove = true;StartCoroutine(wizSpa.SpawnWizards()); player.CanShoot = true; phase_alt++;
                        musicPlayer.musicPlay(musicPlayer.mu_end);
                        break;
                    case 17:
                        phase_alt = 0;
                        phase = "END";
                        break;


                    default: if (click()) { dial_c(); phase_alt++; }; break;

                }

            break;




            case "END":
                switch (phase_alt)
                {
                    case 0: 
                        if (wizSpa.areAllWizardsLikePwnedOrSomething()){
                            wait(2); phase_alt++;
                            musicPlayer.musicFadeOut();
                        }
                        break;
                    case 1:
                        startBgAnimator.SetBool("XDIMENSIO", false);
                        startBgAnimator.SetBool("XDIMENSIO", false);
                        phase_alt++;
                        wait(1);
                        break;
                    case 2:
                        dialMana.StartDialogue(cut_end); phase_alt++;

                        endSprite.SetActive(true);
                        break;

                    default: if (click()) { dial_c(); phase_alt++; }; break;



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

    public void setPhase(string _phase)
    {
        phase = _phase;
    }
}
