using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    public bool platform = false;
	// Use this for initialization
	void Start () {
        if (platform)
        {
            FindObjectOfType<MainController>().onBeat.AddListener(JumpBeat);
        }
        else
        {
            FindObjectOfType<MainController>().onJumpBeat.AddListener(JumpBeat);
        }

    }
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, 0.2f);
    }

    void JumpBeat()
    {
        transform.localScale = Vector2.one * (platform? 1.6f : 1.4f);
    }

}
