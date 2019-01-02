using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;

 using System.IO;
 using System.Linq;
using UnityEngine.UI;

 
 public class MusicPlayer : MonoBehaviour
 {
     public enum SeekDirection { Forward, Backward }
 
     public AudioSource source;
     public List<AudioClip> clips = new List<AudioClip>();
 
     [SerializeField] [HideInInspector] private int currentIndex = 0;
 
     private FileInfo[] soundFiles;
     public List<string> validExtensions = new List<string> { ".ogg", ".wav", ".mp3" }; // Don't forget the "." i.e. "ogg" won't work - cause Path.GetExtension(filePath) will return .ext, not just ext.
     public string absolutePath = "./"; // relative path to where the app is running - change this to "./music" in your case

    [SerializeField] InputField filePath;

     void Start()
     {
         //being able to test in unity
         //if (Application.isEditor) absolutePath = "Assets/";
 
         if (source == null) source = gameObject.AddComponent<AudioSource>();
 
         ReloadSounds();

        GetCurDirFolders();
     }

    string _curPath;
    List<string> _curDirectoryFolderPaths = new List<string>();

    void GetCurDirFolders()
    {
        _curPath = Directory.GetDirectoryRoot("/");
        foreach (string folderPath in Directory.GetDirectories(_curPath))
        {
            try
            {
                _curDirectoryFolderPaths.Add(folderPath);
                Debug.Log(folderPath);
            }
            catch (System.Exception error)
            {
                Debug.Log(error);
            }
        }
        Debug.Log("Found " + _curDirectoryFolderPaths.Count.ToString() + " Folder(s) in this Directory ");
    }

    public void CheckPath()
    {
        absolutePath = filePath.text;
        ReloadSounds();
    }

    void Seek(SeekDirection d)
     {
         if (d == SeekDirection.Forward)
             currentIndex = (currentIndex + 1) % clips.Count;
         else {
             currentIndex--;
             if (currentIndex < 0) currentIndex = clips.Count - 1;
         }
     }
 
     void PlayCurrent()
     {
         source.clip = clips[currentIndex];
         source.Play();
     }
 
     void ReloadSounds()
     {
         clips.Clear();
         // get all valid files
         var info = new DirectoryInfo(absolutePath);
         soundFiles = info.GetFiles()
             .Where(f => IsValidFileType(f.Name))
             .ToArray();

        List<Music> tracks = new List<Music>();

         // and load them
         foreach (var s in soundFiles)
        {
            tracks.Add(new Music(s.Name, s.FullName));
        }

        FindObjectOfType<Selection>().GenerateList(tracks);
        //StartCoroutine(LoadFile(s.FullName));
    }
 
     bool IsValidFileType(string fileName)
     {
         return validExtensions.Contains(Path.GetExtension(fileName));
         // Alternatively, you could go fileName.SubString(fileName.LastIndexOf('.') + 1); that way you don't need the '.' when you add your extensions
     }
 
    public void LoadThatFile(string path)
    {
        Debug.Log(path);
        StartCoroutine(LoadFile(path));
    }

    IEnumerator LoadFile(string path)
     {
         WWW www = new WWW("file://" + path);
         print("loading " + path);
 
         AudioClip clip = www.GetAudioClip(false);
         while(!clip.isReadyToPlay)
             yield return www;
 
         print("done loading");
         clip.name = Path.GetFileName(path);
         FindObjectOfType<LoadedClips>().clips.Add(clip);

     }
 }