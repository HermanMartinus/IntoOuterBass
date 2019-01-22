using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour {

    Slider slider;

    public static float difficulty = 0.6f;

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        slider.value = difficulty/2;
	}
	
	public void UpdateValue () {
        difficulty = slider.value*2;
        Debug.Log(difficulty);
    }
}
