using UnityEngine;
using System.Collections;

public class SoundCloud : MonoBehaviour
{
    private AudioSource _audiopoint;
    public string musicUrl;
    private WWW _www;

    void Start()
    {
        _audiopoint = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine(DownloadAndPlay(musicUrl));
            Debug.Log(Application.dataPath);
            StartCoroutine(DownloadAndPlay("file:////Users/herman/Downloads/01. Low (Dirty).mp3"));
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