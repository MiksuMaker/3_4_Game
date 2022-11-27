using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update

    public static bool playAll = false;

    public void PlayAll()
    {
        playAll = true;
    }

    public void PlayKS()
    {

    }

    public void PlayMiksu()
    {

    }

    public void PlayMilja()
    {
        SceneManager.LoadScene("Milja", LoadSceneMode.Single);
    }

}
