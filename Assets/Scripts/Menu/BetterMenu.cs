using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BetterMenu : MonoBehaviour
{
    public InputField searchInput;
    public GameObject menuItem;
    public Transform grid;
    public GameObject leaderBoard;

    static bool loadedDaftPunk = false;
    public List<string> artists = new List<string>();

    void Start()
    {
        if (!loadedDaftPunk)
        {
            StartCoroutine(LoadDaftPunk());
            loadedDaftPunk = true;
        }
        else
            UpdateMenu();
    }

    IEnumerator LoadDaftPunk()
    {
        if(LoadingScreen.Instance == null)
            yield return new WaitForEndOfFrame();
        Search();
    }

    public void UpdateMenu()
    {
        if (LoadedClips.Instance.searchResults.Count > 0)
        {
            foreach (Track track in LoadedClips.Instance.searchResults)
            {
                GameObject spawnedButton = Instantiate(menuItem, grid);
                spawnedButton.GetComponentsInChildren<Text>()[0].text = track.title;

                string duruation = string.Format("{0:0}:{1:00}", Mathf.Floor(track.duration / 60), track.duration % 60);
                string artist = track.artist != null && track.artist.Length > 0 ? "\nArtist: " + track.artist : "";
                string genre = track.genre != null && track.genre.Length > 0 ? "\nGenre: " + track.genre : "";
                spawnedButton.GetComponentsInChildren<Text>()[1].text = "Duration: " + duruation + artist + genre;

                spawnedButton.transform.Find("Play").GetComponent<Button>().onClick.AddListener(() => LoadTrack(track));
                spawnedButton.transform.Find("Score").GetComponent<Button>().onClick.AddListener(() => Leaderboard(track));
            }
            SetArtwork();
            for (int i=1; i<11; i++)
                Invoke("SetArtwork", i);

        }
        else
        {
          // Empty results
        }

    }

    public void Search()
    {
        if (!MusicLoader.Instance.ConnectedToInternet)
            return;

        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }
        MusicLoader.Instance.SearchForTracks(searchInput.text == "" ? artists[Random.Range(0,artists.Count)] : searchInput.text);
    }

    void SetArtwork()
    {
        if (LoadedClips.Instance.searchResults.Count > 0)
        {
            int i = 0;
            foreach (Track track in LoadedClips.Instance.searchResults)
            {
                if(track.artwork_sprite != null)
                {
                    grid.GetChild(i).Find("Image").GetComponent<Image>().sprite = track.artwork_sprite;
                }
                i++;
            }
        }
    }

    public void LoadTrack(Track selectedTrack)
    {
        if (!MusicLoader.Instance.ConnectedToInternet)
            return;

        LoadedClips.Instance.selectedTrack = selectedTrack;

        if (LoadedClips.Instance.tracks.Find((obj) => obj.song_id == selectedTrack.song_id) == null)
            MusicLoader.Instance.FetchTrack(selectedTrack);
        else
        {
            LoadedClips.Instance.selectedTrack = LoadedClips.Instance.tracks.Find((obj) => obj.song_id == selectedTrack.song_id);
            SongLoaded();
        }
    }

    public void SongLoaded()
    {
        SceneManager.LoadScene("Main");
    }

    public void Leaderboard(Track track)
    {
        LoadedClips.Instance.selectedTrack = track;
        leaderBoard.SetActive(true);
    }

    public void OnInputChange()
    {
        SoundManager.Instance.PlaySoundEffect("Blop", volume: 0.5f);
    }
}
