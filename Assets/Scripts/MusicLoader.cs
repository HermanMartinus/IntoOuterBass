using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

using SimpleJSON;
using System.Linq;
using System;

public class MusicLoader : MonoBehaviour {

    public string searchQuery = "";

    public OnSearchEndEventHandler onSearchEnd;
    [System.Serializable]
    public class OnSearchEndEventHandler : UnityEngine.Events.UnityEvent{}

    public OnLoadEndEventHandler onLoadEnd;
    [System.Serializable]
    public class OnLoadEndEventHandler : UnityEngine.Events.UnityEvent { }

    public Track loadedTrack;

    public static MusicLoader Instance;

    struct Query
    {
        public string q;
        public string song_id;
        public int page_size;
    }

    public void Awake()
    {
        Instance = this;
    }

    public void SearchForTracks(string query)
    {
        StartCoroutine(Search(query));
    }

    IEnumerator Search(string _query)
    {
        LoadingScreen.Instance.Show();

        Debug.Log("Searching " + _query);
        Query query = new Query();
        query.q = _query;
        query.page_size = 10;
        string jsonStringTrial = JsonUtility.ToJson(query);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStringTrial);

        var www = new UnityWebRequest("https://h28j7spcq0.execute-api.us-east-1.amazonaws.com/dev/search");
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(data);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        var N = JSON.Parse(www.downloadHandler.text);

        LoadedClips.Instance.searchResults.Clear();
        for(int i = 0; i<N["response"].Count; i++)
        {
            JSONNode jsonSong = N["response"][i];
            Track track = new Track(jsonSong["song_id"], jsonSong["title"], jsonSong["artist"], jsonSong["genre"], jsonSong["artwork_url"], jsonSong["url"], jsonSong["duration"]);

            LoadedClips.Instance.searchResults.Add(track);

            if(track.artwork_url != null)
                StartCoroutine(LoadArtwork(track));
        }
        onSearchEnd.Invoke();

        LoadingScreen.Instance.Hide();
    }

    public void FetchTrack(Track track)
    {
        loadedTrack = track;
        if (!track.clip)
            StartCoroutine(FetchSongUrl(track.song_id));
        else
            onLoadEnd.Invoke();
    }

    IEnumerator FetchSongUrl(string song_id)
    {
        LoadingScreen.Instance.Show();

        Query query = new Query();
        query.song_id = song_id;
        string jsonStringTrial = JsonUtility.ToJson(query);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStringTrial);

        var www = new UnityWebRequest("https://h28j7spcq0.execute-api.us-east-1.amazonaws.com/dev/fetch_song_info");
        www.method = "POST";
        www.uploadHandler = new UploadHandlerRaw(data);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        var N = JSON.Parse(www.downloadHandler.text);
        loadedTrack.clipUrl = N["response"];
        StartCoroutine(LoadFromUrl(loadedTrack.clipUrl));

    }

    public void LoadThatUrl(string url)
    {
        StartCoroutine(LoadFromUrl(url));
    }

    IEnumerator LoadFromUrl(string url)
    {

        print("loading " + url);
        UnityWebRequest music = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        yield return music.SendWebRequest();


        if (music.isNetworkError)
        {
            Debug.Log(music.error);
        }
        else
        {

            AudioClip clip = DownloadHandlerAudioClip.GetContent(music);
            while (!clip.isReadyToPlay)
                yield return null;

            print("done loading");
            clip.name = loadedTrack.song_id;
            loadedTrack.clip = clip;
            LoadedClips.Instance.tracks.Insert(0, loadedTrack);
            onLoadEnd.Invoke();
        }

        LoadingScreen.Instance.Hide();
    }

    IEnumerator LoadArtwork(Track track)
    {
        WWW www = new WWW(track.artwork_url);
        yield return www;
        Sprite artwork_sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        LoadedClips.Instance.searchResults[LoadedClips.Instance.searchResults.FindIndex((obj) => obj.song_id == track.song_id)].artwork_sprite = artwork_sprite;
    }

    public void GetFiles(string path)
    {
        // "/storage/emulated/0/" -- Android
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
                    Debug.Log(trackName);
                }

            }
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
        StartCoroutine(LoadFromFile(path));
    }

    IEnumerator LoadFromFile(string path)
    {
        WWW www = new WWW("file://" + path);
        print("loading " + path);

        AudioClip clip = www.GetAudioClip(false);
        while (!clip.isReadyToPlay)
            yield return www;

        print("done loading");
        clip.name = Path.GetFileName(path);
    }
}
