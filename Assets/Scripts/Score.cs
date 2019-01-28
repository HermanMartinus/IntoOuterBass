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
    public List<Color> levelColors = new List<Color>();

    bool failing = false;

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

        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, 0.2f);

        GetComponent<Text>().text = score.ToString("000000");
	}

    void Jump()
    {
        if (!mainController.playing)
            return;
        score += Mathf.RoundToInt(jumpPoints * (bassShip.level+1) * Difficulty.difficulty);
        GetComponent<Text>().color = levelColors[bassShip.level];
        if(bassShip.level == 5)
        {
            transform.localScale = Vector2.one * 1.7f;
        }
        else
        {
            transform.localScale = Vector2.one * 1.1f;
        }
    }

    void Fail()
    {
        score -= jumpPoints * (bassShip.level + 1) * 20;
        failing = true;
        GetComponent<Text>().color = Color.red;
        transform.localScale = Vector2.one * 1.7f;
    }
}
