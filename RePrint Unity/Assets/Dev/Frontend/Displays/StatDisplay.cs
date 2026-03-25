using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{

    [SerializeField] private Meter meter;

    [SerializeField] private TextMeshProUGUI fractionText;

    public void UpdateStatDisplay(float numerator, float denominator)
    {
        if (meter && denominator != 0)
        {
            meter.UpdateFill(numerator / denominator);
        }
        fractionText.text = numerator.ToString() + '/' + denominator.ToString();
    }
}
