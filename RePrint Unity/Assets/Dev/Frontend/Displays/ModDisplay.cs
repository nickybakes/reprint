using TMPro;
using UnityEngine;

public class ModDisplay : FloatingDraggableDisplay
{

    /// <summary>
    /// Sets up the rect transform and travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
        SetupDraggable();
    }

    void Start()
    {

    }

    void Update()
    {

    }
}