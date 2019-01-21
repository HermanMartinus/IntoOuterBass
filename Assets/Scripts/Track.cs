using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine;using UnityEngine.Networking;

[System.Serializable]
public class Track {

    public Track (string song_id, string title, string artist, string genre, string artwork_url, string url, float duration)
    {
        this.song_id = song_id;
        this.title = title;
        this.artist = artist;
        this.genre = genre;
        this.artwork_url = artwork_url;
        this.url = url;
        this.duration = duration/1000;
    }

    public string song_id;
    public string title;
    public string artist;
    public string genre;
    public string artwork_url;
    public Sprite artwork_sprite;
    public string url;
    public float duration;
    public string clipUrl;
    public AudioClip clip;
}
