using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Leaderboard : MonoBehaviour {

    public List<HighScore> scores = new List<HighScore>();
    [SerializeField] GameObject scorePrefab;
    [SerializeField] GameObject inputObject;

    int score = 0;

	public void ShowLeaderboard (int _score) {
        score = _score;
        inputObject.SetActive(true);
	}

    public void onNameInput(string value)
    {
        InputField inputField = FindObjectOfType<InputField>();
        inputField.text = inputField.text.ToUpper();
        if (value.Length == 3 && inputObject.activeSelf)
        {
            InsertScore(value);
            inputObject.SetActive(false);
        }
    }

    void DisplayScores(int? scoreIndex = null)
    {
        var index = 0;

        foreach(HighScore score in scores)
        {
            index++;

            GameObject spawnedScore = Instantiate(scorePrefab, transform.Find("Grid"));
            spawnedScore.transform.Find("Rank").GetComponent<Text>().text = score.rank == "" ? AddOrdinal(index) : score.rank;
            spawnedScore.transform.Find("Score").GetComponent<Text>().text = score.points.ToString("00000");
            spawnedScore.transform.Find("Initials").GetComponent<Text>().text = score.initials;
            if(scoreIndex != null && index-1 == scoreIndex)
            {
                spawnedScore.transform.Find("Rank").GetComponent<Text>().color = Color.red;
            }
        }
    }

    void InsertScore(string initials) 
    {
        HighScore newScore = new HighScore(initials, score);
        scores.Add(newScore);
        scores = scores.OrderByDescending(s => s.points).ToList();
        if (scores.Count > 9) 
            scores.RemoveAt(scores.Count-1);

        if (!scores.Contains(newScore))
        {
            scores[scores.Count-1] = newScore;
            scores[scores.Count - 1].rank = "...";
        }

        DisplayScores(scores.IndexOf(newScore));
    }

    void SaveScore()
    {

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

[System.Serializable]
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