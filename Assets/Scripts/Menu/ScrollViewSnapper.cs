using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewSnapper : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{

    public float SnapSpeed = 10;
    public float ScrollVelocity;

    private bool isDragging;
    public int Snaps;
    public static bool Dragging;

    [SerializeField] Scrollbar scroll;
    [SerializeField] ScrollRect sRect;
    [SerializeField] Transform content;

    public static int selectedIndex = 0;

    public OnSelectedItem onSelect;
    [Serializable]
    public class OnSelectedItem : UnityEngine.Events.UnityEvent {}

    void Start()
    {
        Snaps = content.childCount;
        SelectTheOne();
    }

    // Update is called once per frame
    void Update()
    {
        isDragging = Dragging;

        if (!scroll)
            return;

        if (!Dragging)
            scroll.value = Mathf.Lerp(scroll.value, Mathf.Round(scroll.value * (Snaps - 1)) / (Snaps - 1), SnapSpeed * Time.deltaTime);
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Snaps = content.childCount;
        Dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Dragging = false;
        SelectTheOne();
    }

    void SelectTheOne()
    {
        selectedIndex = content.childCount - Mathf.RoundToInt(scroll.value * (Snaps - 1)) - 1;
        onSelect.Invoke();
    }
}