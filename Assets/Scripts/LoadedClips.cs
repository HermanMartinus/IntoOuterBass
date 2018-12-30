using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedClips : MonoBehaviour {

    public List<AudioClip> clips = new List<AudioClip>();

    private void Awake()
    {
        Selection[] objs = FindObjectsOfType<Selection>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
