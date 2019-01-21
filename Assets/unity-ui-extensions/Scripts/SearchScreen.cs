using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

namespace UnityEngine.UI.Extensions.Examples
{
    public class SearchScreen : MonoBehaviour
    {
        [SerializeField] SearchScrollView scrollView;
        [SerializeField] MusicLoader musicLoader;
        [SerializeField] InputField searchInput;
        [SerializeField] Text durationText;
        [SerializeField] Image artworkImage;

        public static Track selectedSong;

        private void Start()
        {
            UpdateMenu();
        }

        public void Search()
        {
            musicLoader.SearchForTracks(searchInput.text);
        }

        public void UpdateMenu()
        {
            List<Track> tracks = musicLoader.searchResults;
            var cellData = Enumerable.Range(0, tracks.Count)
                .Select(i => tracks[i])
                .ToList();

            scrollView.UpdateData(cellData);
        }

        public void OnItemSelected()
        {
            string duruation = string.Format("{0:0}:{1:00}", Mathf.Floor(selectedSong.duration / 60), selectedSong.duration % 60);
            durationText.text = "Duration: " + duruation;
            artworkImage.sprite = selectedSong.artwork_sprite;
        }

        public void LoadSong()
        {
            musicLoader.FetchTrack(selectedSong);
        }

        public void SongLoaded()
        {
            SceneManager.LoadScene("Main");
        }
    }
}

