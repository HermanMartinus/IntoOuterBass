﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Selection : MonoBehaviour {

    public AudioClip[] resourceAudioClips;
    [SerializeField] GameObject button;
    [SerializeField] Transform grid;
    public AudioClip selectedMusic;

    public void GenerateList(List<Music> tracks)
    {

        foreach (Music track in tracks)
        {
            GameObject spawnedButton = Instantiate(button, grid);
            spawnedButton.GetComponentInChildren<Text>().text = track.name;
            spawnedButton.GetComponent<Button>().onClick.AddListener(delegate { AddTrack(track.url); });
        }
    }

    public void AddTrack(string url)
    {
        Debug.Log(url);

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
        Debug.Log(scene.name);
        if(scene.name == "Selection")
        {
            grid.gameObject.SetActive(true);
        }
    }
}

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