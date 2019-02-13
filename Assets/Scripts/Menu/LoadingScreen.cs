using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public static LoadingScreen Instance;

    [SerializeField] GameObject content;

    public List<string> tips = new List<string>();

	// Use this for initialization
	void Start () {
        Instance = this;
        content.SetActive(false);
    }
	
	public void Show () {
        content.GetComponentInChildren<Text>().text = tips[Random.Range(0, tips.Count)];
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

    public void ChangeText()
    {
        content.GetComponentInChildren<Text>().text = tips[Random.Range(0, tips.Count)];
    }

    public void Cancel()
    {
        content.SetActive(false);
        MusicLoader.Instance.StopAllCoroutines();
    }
}
