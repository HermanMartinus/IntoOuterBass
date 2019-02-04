using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SearchMenu : MonoBehaviour
{

    [SerializeField] GameObject cassettePrefab;
    [SerializeField] Transform scrollContent;
    [SerializeField] InputField searchInput;
    [SerializeField] Text decorationText;
    [SerializeField] Image artworkImage;
    [SerializeField] Sprite artworkPlaceholder;
    [SerializeField] GameObject leaderboard;

    public Track selectedTrack;

    // Use this for initialization
    void Start()
    {
        UpdateMenu();
    }

    public void Search()
    {
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject);
        }
        MusicLoader.Instance.SearchForTracks(searchInput.text);
    }

    public void UpdateMenu()
    {
        if(LoadedClips.Instance.searchResults.Count > 0)
        {
            LoadedClips.Instance.selectedTrack = LoadedClips.Instance.searchResults[0];
            SetDecorations();

            foreach (Track track in LoadedClips.Instance.searchResults)
            {
                GameObject spawnedButton = Instantiate(cassettePrefab, scrollContent);
                spawnedButton.GetComponentInChildren<Text>().text = track.title;
                spawnedButton.transform.Find("Backer").GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
        }
        else
        {
            decorationText.text = "No results...";
            artworkImage.sprite = artworkPlaceholder;
        }

    }

    public void OnItemSelected()
    {
        LoadedClips.Instance.selectedTrack = LoadedClips.Instance.searchResults[ScrollViewSnapper.selectedIndex];
        SetDecorations();
        SoundManager.Instance.PlaySoundEffect("ButtonClick", volume: 0.5f);
    }

    void SetDecorations()
    {
        selectedTrack = LoadedClips.Instance.selectedTrack;
        string duruation = string.Format("{0:0}:{1:00}", Mathf.Floor(selectedTrack.duration / 60), selectedTrack.duration % 60);
        string artist = selectedTrack.artist != null && selectedTrack.artist.Length > 0 ? "\nArtist: " + selectedTrack.artist : "";
        string genre = selectedTrack.genre != null && selectedTrack.genre.Length > 0 ? "\nGenre: " + selectedTrack.genre : "";
        decorationText.text = "Duration: " + duruation + artist + genre + "\n" + selectedTrack.url;
        if (selectedTrack.artwork_sprite == null)
        {
            artworkImage.sprite = artworkPlaceholder;
        }
        else
        {
            artworkImage.sprite = selectedTrack.artwork_sprite;
        }
    }

    public void OnInputChange()
    {
        SoundManager.Instance.PlaySoundEffect("Blop", volume:0.5f);
    }

    public void LoadSong()
    {
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
        leaderboard.SetActive(true);
    }
}