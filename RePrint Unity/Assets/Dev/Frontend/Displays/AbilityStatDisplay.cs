using TMPro;
using UnityEngine;

public class AbilityStatDisplay : Display
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

    public void DisplayStat(int min, int max)
    {
        // gameObject.SetActive(!(min == 0 && max == 0));

        if (min == max)
        {
            string finalString = singleValueFormat;
            while (finalString.Contains("%a"))
            {
                finalString = finalString.Replace("%a", min.ToString());
            }
            numberText.SetText(finalString);
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
            numberText.SetText(finalString);
        }

    }
}