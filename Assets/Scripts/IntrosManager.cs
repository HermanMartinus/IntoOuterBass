using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntrosManager : MonoBehaviour {

    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip videoClip;

	// Use this for initialization
	void Start () {
        videoPlayer.clip = videoClip;
        Invoke("EndScene", (float)videoClip.length);
	}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EndScene();
        }
    }


    void EndScene()
    {
        SceneManager.LoadScene("SearchMenu");
    }
}
