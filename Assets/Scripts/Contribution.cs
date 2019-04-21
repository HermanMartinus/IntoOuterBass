using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Contribution : MonoBehaviour
{
    public GameObject everthing;
    public GameObject thanks;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("contributed") == "true")
        {
            Skip();
        }
    }

    public void Contribute ()
    {
        Application.OpenURL("https://www.buymeacoffee.com/AdmtxiM4N");
        thanks.SetActive(true);
        everthing.SetActive(false);
    }

    public void AlreadyContributed ()
    {
        thanks.SetActive(true);
        everthing.SetActive(false);
        PlayerPrefs.SetString("contributed", "true");
    }

    public void Failed()
    {
        Debug.Log("Hmmmm, something went wrong with the contribution :/");
    }

    public void Skip ()
    {
        SceneManager.LoadScene("SearchMenu");
    }
}
