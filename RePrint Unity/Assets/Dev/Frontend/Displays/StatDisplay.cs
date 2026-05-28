using TMPro;
using UnityEngine;

public class StatDisplay : Display
{

    [SerializeField] private Meter meter;

    [SerializeField] private TextDisplay textDisplay;

    [SerializeField] private DifferenceDisplayPool differenceDisplayPool;

    [SerializeField] private string singleValueFormat = "%a";

    [SerializeField] private string fractionValueFormat = "%a/%b";

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
        string finalString = singleValueFormat;
        while (finalString.Contains("%a"))
        {
            finalString = finalString.Replace("%a", value.ToString());
        }

        textDisplay.SetText(finalString);

        DisplayDifference(value);
    }

    public void DisplayFraction(float numerator, float denominator)
    {
        if (meter && denominator != 0)
        {
            meter.UpdateFill(numerator / denominator);
        }

        string finalString = fractionValueFormat;
        while (finalString.Contains("%a"))
        {
            finalString = finalString.Replace("%a", numerator.ToString());
        }
        while (finalString.Contains("%b"))
        {
            finalString = finalString.Replace("%b", denominator.ToString());
        }

        textDisplay.SetText(finalString);

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
