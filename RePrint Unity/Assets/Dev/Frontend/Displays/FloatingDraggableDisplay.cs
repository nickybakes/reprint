using System;
using UnityEngine;
using UnityEngine.Events;

public class FloatingDraggableDisplay : FloatingDisplay
{
    private FloatingDraggableGroup group;

    public FloatingDraggableGroup Group { get => group; }

    private int indexInGroup;
    public int IndexInGroup { get => indexInGroup; }


    private BetterDraggable draggable;

    public void SetGroup(FloatingDraggableGroup _group, int _indexInGroup)
    {
        group = _group;
        indexInGroup = _indexInGroup;
    }


    /// <summary>
    /// Sets up the rect transform and travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
        draggable = GetComponent<BetterDraggable>();
        draggable.StartDragEvent.AddListener(StartDrag);
        draggable.StopDragEvent.AddListener(StopDrag);
    }

    private void StartDrag()
    {
        SetStartTransform(rectTransform);
        StartTraveling();
        group.StartDragFromElement(this);
    }

    private void StopDrag()
    {
        SetStartTransform(rectTransform);
        group.StopDragFromElement(this);
        StartTraveling();
    }

    void Update()
    {
        UpdateTravel();
    }

}