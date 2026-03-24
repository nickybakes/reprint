using UnityEngine;

public class CharacterFigure : FloatingFigure
{
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
