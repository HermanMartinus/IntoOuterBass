using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyHandler : MonoBehaviour {

    public float modifier = 2;
	// Use this for initialization
	void Start () {
        modifier = Difficulty.difficulty * 4;
        MainController.Instance.boxSpeed *= modifier;
        MainController.Instance.platformDropTimer /= modifier;
        BaseShip.Instance.jumpTime /=  modifier;
        BeatManager.Instance.timeDifference /= modifier;
        BeatManager.Instance.cooldownTime /= modifier;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
