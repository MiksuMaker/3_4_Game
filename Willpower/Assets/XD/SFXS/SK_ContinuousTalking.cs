using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_ContinuousTalking : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioClip au;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(soundPlay());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator soundPlay()
    {
        yield return new WaitForSeconds((float)0.1);
        audioSource.pitch = Random.Range((float)0.9, (float)1.1);
        audioSource.PlayOneShot(au);
        StartCoroutine(soundPlay());
    }
}
