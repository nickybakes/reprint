using System;
using UnityEngine;

public class FloatingFigure : TravelingFigure
{
    /// <summary>
    /// Transition lerp between unselected and selected size. Use the animator to control this.
    /// </summary>
    [SerializeField, Range(0.0f, 1.0f)] private float sizeTransition;

    /// <summary>
    /// Transition lerp between unselected and selected size. Use the animator to control this.
    /// </summary>
    public float SizeTransition { get => sizeTransition; }

    /// <summary>
    /// Sets up the travel data.
    /// </summary>
    void Awake()
    {
        SetupTravelingTransformData();
    }

    void Update()
    {
        UpdateTravel();
    }

}