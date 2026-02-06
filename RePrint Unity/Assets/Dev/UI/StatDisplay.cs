using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{

    [SerializeField]
    private UIMeter meter;

    [SerializeField]
    private TextMeshProUGUI fractionText;

    public void UpdateStatDisplay(float numerator, float denominator)
    {
        meter.UpdateFill(numerator / denominator);
        fractionText.text = numerator.ToString() + '/' + denominator.ToString();
    }
}
