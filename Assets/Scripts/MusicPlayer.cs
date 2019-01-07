using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System;

public class MusicPlayer : MonoBehaviour
{
    public enum SeekDirection { Forward, Backward }

    public AudioSource source;
    public List<AudioClip> clips = new List<AudioClip>();

    [SerializeField] [HideInInspector] private int currentIndex = 0;

    LoadedClips loadedClips;

    private FileInfo[] soundFiles;
    public string absolutePath = "/"; // relative path to where the app is running - change this to "./music" in your case
    public string androidPath = "/storage/emulated/0/";
    public string iOSPath = "/private/var/mobile";

    [SerializeField] InputField filePath;

    void Start()
    {
        loadedClips = FindObjectOfType<LoadedClips>();
        if (loadedClips.audioFiles.Count == 0)
        {
            if (source == null) source = gameObject.AddComponent<AudioSource>();
            if (Application.platform == RuntimePlatform.Android)
                GetFiles(androidPath);
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                GetFiles(iOSPath);
            else
                GetFiles(absolutePath);
        }
        else
        {
            FindObjectOfType<Selection>().GenerateList(loadedClips.audioFiles);
        }
    }

    void Seek(SeekDirection d)
    {
        if (d == SeekDirection.Forward)
            currentIndex = (currentIndex + 1) % clips.Count;
        else
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = clips.Count - 1;
        }
    }

    void PlayCurrent()
    {
        source.clip = clips[currentIndex];
        source.Play();
    }

    public void GetFiles(string path)
    {
        List<string> fileList = new List<string>();

        try
        {

            IEnumerable<string> files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".mp3") || s.EndsWith(".wav") || s.EndsWith(".aif") || s.EndsWith(".ogg"));
                
            foreach (string f in files)
            {
                FileInfo file = new FileInfo(f);
                long fileSize = file.Length;
                if (fileSize / 1024 > 1024)
                {
                    string trackName = f.Substring(f.LastIndexOf('/') + 1);
                    trackName = trackName.Substring(0, trackName.LastIndexOf('.'));
                    loadedClips.audioFiles.Add(new Music(trackName, f));
                }

            }
            FindObjectOfType<Selection>().GenerateList(loadedClips.audioFiles);
        }
        catch (UnauthorizedAccessException UAEx)
        {
            Console.WriteLine(UAEx.Message);
        }
        catch (PathTooLongException PathEx)
        {
            Console.WriteLine(PathEx.Message);
        }

    }

    public void LoadThatFile(string path)
    {
        StartCoroutine(LoadFile(path));
    }

    IEnumerator LoadFile(string path)
    {
        WWW www = new WWW("file://" + path);
        print("loading " + path);

        AudioClip clip = www.GetAudioClip(false);
        while (!clip.isReadyToPlay)
            yield return www;

        print("done loading");
        clip.name = Path.GetFileName(path);
        loadedClips.clips.Add(clip);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}