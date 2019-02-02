using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour {

    public List<HighScore> scores = new List<HighScore>();
    [SerializeField] GameObject scorePrefab;
    [SerializeField] GameObject inputObject;
    [SerializeField] GameObject retryButton;
    [SerializeField] GameObject menuButton;

    int points = 0;
    public bool viewing = false;

    private void OnEnable()
    {
        if (viewing)
        {
            ShowLeaderboard();
            inputObject.SetActive(false);
            retryButton.SetActive(false);
            menuButton.SetActive(true);
        }

        GetScore();

        if (viewing)
        {
            DisplayScores();
        }
    }

    public void ShowLeaderboard (int _points = 0) {
        points = _points;
        inputObject.SetActive(true);
        PopulateStats(points);
	}

    public void PopulateStats (int points)
    {
        if(! viewing)
            inputObject.transform.Find("Stats").GetComponent<Text>().text = LoadedClips.Instance.selectedTrack.title + "\n" + points.ToString("000000");
    }

    public void InputLetter(string letter)
    {
        InputField inputField = FindObjectOfType<InputField>();
        inputField.text += letter;
        if (inputField.text.Length == 3 && inputObject.activeSelf)
        {
            InsertScore(inputField.text);
            inputObject.SetActive(false);
        }
    }

    public void RemoveLetter()
    {
        InputField inputField = FindObjectOfType<InputField>();
        if(inputField.text.Length > 0)
            inputField.text = inputField.text.Remove(inputField.text.Length-1);
    }

    void DisplayScores(int? scoreIndex = null)
    {
        var index = 0;

        foreach(HighScore score in scores)
        {
            index++;

            GameObject spawnedScore = Instantiate(scorePrefab, transform.Find("Grid"));
            spawnedScore.transform.Find("Rank").GetComponent<Text>().text = score.rank == "" ? AddOrdinal(index) : score.rank;
            spawnedScore.transform.Find("Score").GetComponent<Text>().text = score.points.ToString("000000");
            spawnedScore.transform.Find("Initials").GetComponent<Text>().text = score.initials;
            if(scoreIndex != null && index-1 == scoreIndex)
            {
                spawnedScore.transform.Find("Rank").GetComponent<Text>().color = Color.magenta;
                spawnedScore.transform.Find("Score").GetComponent<Text>().color = Color.magenta;
                spawnedScore.transform.Find("Initials").GetComponent<Text>().color = Color.magenta;
            }
        }

        if(!viewing)
            retryButton.SetActive(true);
        menuButton.SetActive(true);
    }

    void InsertScore(string initials) 
    {
        HighScore newScore = new HighScore(initials, points);
        scores.Add(newScore);
        scores = scores.OrderByDescending(s => s.points).ToList();
        if (scores.Count > 10) 
            scores.RemoveAt(scores.Count-1);

        if (!scores.Contains(newScore))
        {
            scores[scores.Count-1] = newScore;
            scores[scores.Count - 1].rank = "...";
        }
        SetScore();
        DisplayScores(scores.IndexOf(newScore));
        GetScore();
    }

    void SetScore()
    {
       PlayerPrefs.SetString(LoadedClips.Instance.selectedTrack.song_id, ToJson(scores.ToArray()));
    }

    void GetScore()
    {
        Debug.Log(LoadedClips.Instance.selectedTrack.song_id);
        if (PlayerPrefs.HasKey(LoadedClips.Instance.selectedTrack.song_id))
        {
            HighScore[] retrieved = FromJson<HighScore>(PlayerPrefs.GetString(LoadedClips.Instance.selectedTrack.song_id));
            scores = retrieved.ToList();
        }
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public void Menu ()
    {
        if(viewing)
        {
            foreach(Transform t in transform.Find("Grid"))
            {
                Destroy(t.gameObject);
            }
            scores = new List<HighScore>();
            gameObject.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("SearchMenu");
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static string AddOrdinal(int num)
    {
        if (num <= 0) return num.ToString();

        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        switch (num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }

    }

}

[Serializable]
public class HighScore
{
    public string initials;
    public int points;
    public string rank = "";
    public HighScore(string _initials, int _points)
    {
        initials = _initials;
        points = _points;
    }
}