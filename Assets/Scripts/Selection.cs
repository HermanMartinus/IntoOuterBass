using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour {

    public AudioClip[] resourceAudioClips;
    [SerializeField] GameObject button;
    [SerializeField] Transform grid;
    public AudioClip selectedMusic;

    private void Awake()
    {
        Selection[] objs = FindObjectsOfType<Selection>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        resourceAudioClips = Resources.LoadAll<AudioClip>("Music");
        foreach (AudioClip clip in resourceAudioClips)
        {
            GameObject spawnedButton = Instantiate(button, grid);
            spawnedButton.GetComponentInChildren<Text>().text = clip.name;
            spawnedButton.GetComponent<Button>().onClick.AddListener(delegate { SetTrack(clip); });
        }
    }

    public void SetTrack(AudioClip clip)
    {
        grid.gameObject.SetActive(false);
        selectedMusic = clip;
        SceneManager.LoadScene("Main");
    }


    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if(scene.name == "Selection")
        {
            grid.gameObject.SetActive(true);
        }
    }
}
