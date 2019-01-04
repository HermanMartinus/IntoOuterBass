using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int score = 0;
    public int jumpPoints = 5;

    BaseShip bassShip;
    MainController mainController;

	// Use this for initialization
	void Start () {
        mainController = FindObjectOfType<MainController>();
        bassShip = FindObjectOfType<BaseShip>();
        bassShip.onJump.AddListener(Jump);
        bassShip.onFail.AddListener(Fail);
    }

    float interval = 0.2f;
    float nextTime = 0;

    void Update () {
        if (!mainController.started)
            return;

        GetComponent<Text>().text = score.ToString("00000");
        if (Time.time >= nextTime)
        {
            score += bassShip.level + 1;
            nextTime += interval;
        }
	}

    void Jump()
    {
        if (!mainController.started)
            return;
        score += jumpPoints * (bassShip.level+1);
    }

    void Fail()
    {
        score -= jumpPoints * (bassShip.level + 1) * 20;
    }
}
