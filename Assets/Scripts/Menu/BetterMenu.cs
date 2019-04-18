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
                spawnedButton.transform.Find("Score").GetComponent<Button>().onClick.AddListener(() => Leaderboard());
            }
            SetArtwork();
            Invoke("SetArtwork", 1);
            Invoke("SetArtwork", 3);
            Invoke("SetArtwork", 5);
            Invoke("SetArtwork", 10);
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
        MusicLoader.Instance.SearchForTracks(searchInput.text == "" ? "Daft Punk" : searchInput.text);
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

    public void Leaderboard()
    {
        leaderBoard.SetActive(true);
    }

    public void OnInputChange()
    {
        SoundManager.Instance.PlaySoundEffect("Blop", volume: 0.5f);
    }
}
