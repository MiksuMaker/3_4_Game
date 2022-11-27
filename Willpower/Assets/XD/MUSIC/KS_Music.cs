using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Music : MonoBehaviour
{
    [SerializeField] AudioSource musicPlayer;

    public AudioClip mu_start;
    public AudioClip mu_mid;
    public AudioClip mu_end;


    void Start()
    {
        musicPlay(mu_mid);
        musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { musicPlay(mu_mid); }
        if (Input.GetKeyDown(KeyCode.R)) { musicPlay(mu_end); }
    }


    public void musicPlay(AudioClip musicClip)
    {
        musicPlayer.Stop();
        musicPlayer.clip = musicClip;
        musicPlayer.Play();
    }

    public void musicStop()
    {
        musicPlayer.Stop();
    }
}
