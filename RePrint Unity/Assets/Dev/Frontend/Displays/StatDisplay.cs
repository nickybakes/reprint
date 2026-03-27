using TMPro;
using UnityEngine;

public class StatDisplay : Display
{

    [SerializeField] private Meter meter;

    [SerializeField] private TextDisplay textDisplay;

    [SerializeField] private DifferenceDisplayPool differenceDisplayPool;

    private float currentValue;

    private bool valueNotSet;

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    public void DisplayValue(float value)
    {
        textDisplay.SetText(value.ToString());
        DisplayDifference(value);
    }

    public void DisplayFraction(float numerator, float denominator)
    {
        if (meter && denominator != 0)
        {
            meter.UpdateFill(numerator / denominator);
        }
        textDisplay.SetText(numerator.ToString() + '/' + denominator.ToString());

        DisplayDifference(numerator);
    }

    public void DisplayDifference(float value)
    {
        if (valueNotSet && currentValue != value)
        {
            if (differenceDisplayPool)
            {
                differenceDisplayPool.AddText((int)currentValue, (int)value);
            }
        }
        currentValue = value;
        valueNotSet = true;
    }
}
