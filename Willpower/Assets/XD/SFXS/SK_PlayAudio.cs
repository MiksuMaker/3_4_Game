using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_PlayAudio : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;

    public AudioClip walkSO;
    public AudioClip jumpSO;
    public AudioClip policeSO;
    public AudioClip POWso;
    public AudioClip ripSo;
    
    
  


    public void sound(AudioClip _so)
    {
            audioSource.pitch = Random.Range((float)0.9, (float)1.1);
            audioSource.PlayOneShot(_so);
        
    }
}
