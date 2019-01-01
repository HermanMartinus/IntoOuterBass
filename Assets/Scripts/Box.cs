using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    [SerializeField] GameObject explosion;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(transform.localScale.x > 1)
        {
            transform.localScale *= 0.98f;
        }
    }

}
