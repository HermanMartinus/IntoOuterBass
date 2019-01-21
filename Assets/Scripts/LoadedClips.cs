using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedClips : MonoBehaviour {

    public List<AudioClip> clips = new List<AudioClip>();
    public List<Music> audioFiles = new List<Music>();
    public List<Track> tracks = new List<Track>();

    public static LoadedClips Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
