using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace UnityEngine.UI.Extensions.Examples
{
    public class SearchScreen : MonoBehaviour
    {
        [SerializeField] SearchScrollView scrollView;
        [SerializeField] MusicLoader musicLoader;
        [SerializeField] InputField searchInput;

        public static Track selectedSong;

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

