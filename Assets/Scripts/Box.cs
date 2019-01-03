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
		if(transform.localScale.x > 1)
        {
            transform.localScale *= 0.98f;
        }
    }

    void JumpBeat()
    {
        transform.localScale = Vector2.one * 1.2f;
    }

}
