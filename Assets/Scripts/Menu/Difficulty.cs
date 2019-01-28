using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour {

    Slider slider;

    public static float difficulty = 5f;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        slider.value = difficulty;
	}
	
	public void UpdateValue () {
        difficulty = slider.value;
        SoundManager.Instance.PlaySoundEffect("Click");
    }
}
