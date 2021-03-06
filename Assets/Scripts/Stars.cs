﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour {

    public float speed = 0.3f;
    public float resetOffset = -35f;
    public bool rotated = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(rotated ? Vector3.back * Time.deltaTime * speed : Vector3.down * Time.deltaTime * speed);
        if(transform.position.y < resetOffset)
        {
            transform.position = new Vector3(0, 35, 1);
        }
    }
}
