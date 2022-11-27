using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Music : MonoBehaviour
{
    [SerializeField] AudioSource musicPlayer;

    public AudioClip mu_start;
    public AudioClip mu_mid;
    public AudioClip mu_end;
    float start_volume = 0;


    void Start()
    {
        musicPlay(mu_mid);
        musicPlayer.Play();
        start_volume = musicPlayer.volume;
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


    public void musicFadeOut()
    {
        StartCoroutine(FadeOut(musicPlayer, 3, start_volume));
    }


    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime, float _start_volume)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= _start_volume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
    }


}
