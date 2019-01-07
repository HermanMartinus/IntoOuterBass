using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedClips : MonoBehaviour {

    public List<AudioClip> clips = new List<AudioClip>();
    public List<Music> audioFiles = new List<Music>();

    private void Awake()
    {
        LoadedClips[] objs = FindObjectsOfType<LoadedClips>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
