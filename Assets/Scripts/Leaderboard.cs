﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour {

    public List<HighScore> scores = new List<HighScore>();
    [SerializeField] GameObject scorePrefab;
    [SerializeField] GameObject inputObject;

    int points = 0;
    string trackName = "";

    private void Start()
    {
        GetScore();
    }

    public void ShowLeaderboard (int _points, string _trackName) {
        points = _points;
        trackName = _trackName;
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
                spawnedScore.transform.Find("Score").GetComponent<Text>().color = Color.red;
                spawnedScore.transform.Find("Initials").GetComponent<Text>().color = Color.red;
            }
        }
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
       PlayerPrefs.SetString(trackName, ToJson(scores.ToArray()));
    }
    void GetScore()
    {
        if (PlayerPrefs.HasKey(trackName))
        {
            HighScore[] retrieved = FromJson<HighScore>(PlayerPrefs.GetString(trackName));
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
        if (FindObjectOfType<LoadedClips>())
        {
            foreach (AudioClip clip in FindObjectOfType<LoadedClips>().clips)
            {
                clip.UnloadAudioData();
            }
            FindObjectOfType<LoadedClips>().clips.Clear();
        }
        SceneManager.LoadScene("Selection");
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