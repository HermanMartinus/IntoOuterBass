using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBar : MonoBehaviour {

    Canvas loadingBarCanvas;

    private void Awake()
    {
        loadingBarCanvas = GetComponent<Canvas>();
        loadingBarCanvas.enabled = false;
    }

    public void ShowLoadingBar()
    {
        loadingBarCanvas.enabled = true;
    }

    public void HideLoadingBar()
    {
        loadingBarCanvas.enabled = false;
    }
}
