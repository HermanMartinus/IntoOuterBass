using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour {

    Slider slider;

    public static float difficulty = 5f;

    string[] levels = { "Super easy", "Easy", "Medium", "Challenging", "Hard", "Brutal" };

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.HasKey("difficulty"))
        {
            difficulty = PlayerPrefs.GetFloat("difficulty");
        }
        slider = GetComponent<Slider>();
        slider.value = difficulty;
        GetComponentInChildren<Text>().text = levels[(int)slider.value - 4];
	}
	
	public void UpdateValue () {
        difficulty = slider.value;
        SoundManager.Instance.PlaySoundEffect("Click");
        GetComponentInChildren<Text>().text = levels[(int)slider.value - 4];
        PlayerPrefs.SetFloat("difficulty", difficulty);
    }
}
