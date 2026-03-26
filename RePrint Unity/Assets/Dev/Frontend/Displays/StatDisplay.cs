using TMPro;
using UnityEngine;

public class StatDisplay : Display
{

    [SerializeField] private Meter meter;

    [SerializeField] private TextDisplay fractionText;

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    public void UpdateStatDisplay(float numerator, float denominator)
    {
        if (meter && denominator != 0)
        {
            meter.UpdateFill(numerator / denominator);
        }
        fractionText.SetText(numerator.ToString() + '/' + denominator.ToString());
    }
}
