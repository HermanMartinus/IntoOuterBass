using UnityEngine;
using System.Collections;

public class SoundCloud : MonoBehaviour
{
    private AudioSource _audiopoint;
    private WWW _www;

    void Start()
    {
        _audiopoint = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DownloadAndPlay("https://soundcloud.com/user-107006161/dc-rpg-the-hero-points-podcast-episode-1/download.mp3"));
            //StartCoroutine(DownloadAndPlay("file:///" + Application.dataPath + "/UserSounds/" + "/my.ogg"));
        }
    }
    IEnumerator DownloadAndPlay(string url)
    {
        Debug.Log(url);
        _www = new WWW(url);
        Debug.Log("Start Download");
        yield return _www;
        Debug.Log("Playing");
        _audiopoint = GetComponent<AudioSource>();
        _audiopoint.clip = NAudioPlayer.FromMp3Data(_www.bytes);
        _audiopoint.clip.name = "Downloaded clip";
        _audiopoint.Play();
    }
}