using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int score = 0;
    public int jumpPoints = 10;

    BaseShip bassShip;
    MainController mainController;
    bool altenator = false;

	// Use this for initialization
	void Start () {
        mainController = FindObjectOfType<MainController>();
        bassShip = FindObjectOfType<BaseShip>();
        bassShip.onJump.AddListener(Jump);
        bassShip.onFail.AddListener(Fail);
    }

    void FixedUpdate () {
        if (!mainController.playing)
            return;
        if (altenator)
        {
            score += 1;
        }
        altenator = !altenator;

        GetComponent<Text>().text = score.ToString("00000");
	}

    void Jump()
    {
        if (!mainController.playing)
            return;
        score += jumpPoints * (bassShip.level+1);
    }

    void Fail()
    {
        score -= jumpPoints * (bassShip.level + 1) * 20;
    }
}
