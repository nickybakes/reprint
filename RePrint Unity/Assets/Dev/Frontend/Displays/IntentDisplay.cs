using TMPro;
using UnityEngine;

public class IntentDisplay : Display
{

    [SerializeField] private TextDisplay nameText;
    [SerializeField] private TextDisplay numberText;

    [SerializeField] private string singleValueFormat = "%a";
    [SerializeField] private string rangeValueFormat = "%a - %b";

    private Ability ability;

    void Awake()
    {
        SetupRectTransform();
    }

    public void DisplayIntent(int min, int max)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (min == max)
        {
            string finalString = singleValueFormat;
            while (finalString.Contains("%a"))
            {
                finalString = finalString.Replace("%a", min.ToString());
            }
            numberText.SetText(finalString, true);
        }
        else
        {
            string finalString = rangeValueFormat;
            while (finalString.Contains("%a"))
            {
                finalString = finalString.Replace("%a", min.ToString());
            }
            while (finalString.Contains("%b"))
            {
                finalString = finalString.Replace("%b", max.ToString());
            }
            numberText.SetText(finalString, true);
        }

    }
}