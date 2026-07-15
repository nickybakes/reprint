using System;
using UnityEngine;

public class FloatingDraggableDisplay : FloatingDisplay
{


    /// <summary>
    /// Sets up the rect transform and travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
    }

    void Update()
    {
        UpdateTravel();
    }

}