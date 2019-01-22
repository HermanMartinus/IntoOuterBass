using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public static LoadingScreen Instance;

    [SerializeField] GameObject content;

	// Use this for initialization
	void Start () {
        Instance = this;
        content.SetActive(false);
    }
	
	public void Show () {
        content.SetActive(true);
    }

    public void Hide()
    {
        content.SetActive(false);
    }

    public void Toggle()
    {
        if(content.activeSelf)
        {
            content.SetActive(false);
        }
        else
        {
            content.SetActive(true);
        }
    }
}
