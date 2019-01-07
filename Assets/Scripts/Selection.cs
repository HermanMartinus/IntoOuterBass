using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;

public class Selection : MonoBehaviour {

    public AudioClip[] resourceAudioClips;
    [SerializeField] GameObject button;
    [SerializeField] Transform grid;
    public AudioClip selectedMusic;
    public List<Transform> buttons = new List<Transform>();

    private void Start()
    {
        FindObjectOfType<AudioProcessor>().onBeat.AddListener(Beat);
    }

    void Update()
    {
        foreach(Transform btn in buttons)
        {
            if (btn.localScale.x > 1)
            {
                btn.localScale *= 0.98f;
            }
        }
    }

    void Beat()
    {
        foreach (Transform btn in buttons)
        {
            btn.localScale = Vector2.one * 1.02f;
        }
    }

    public void GenerateList(List<Music> tracks)
    {

        foreach (Music track in tracks)
        {
            GameObject spawnedButton = Instantiate(button, grid);
            spawnedButton.GetComponentInChildren<Text>().text = track.name;
            spawnedButton.GetComponent<Button>().onClick.AddListener(delegate { AddTrack(track.url); });
            spawnedButton.transform.Find("Backer").GetComponent<Image>().color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            buttons.Add(spawnedButton.transform);
        }
    }

    public void AddTrack(string url)
    {
        FindObjectOfType<MusicPlayer>().LoadThatFile(url);
    }

    public void SetTrack()
    {
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
        if(scene.name == "Selection")
        {
            grid.gameObject.SetActive(true);
        }
    }
}

[Serializable]
public struct Music
{
    public Music(string _name, string _url)
    {
        name = _name;
        url = _url;
    }

    public string name { get; private set; }
    public string url { get; private set; }
}