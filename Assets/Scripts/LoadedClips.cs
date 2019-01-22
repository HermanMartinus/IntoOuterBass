using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedClips : MonoBehaviour {

    public List<Track> tracks = new List<Track>();
    public Track selectedTrack;

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
