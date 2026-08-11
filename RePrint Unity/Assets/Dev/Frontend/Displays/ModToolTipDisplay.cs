

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModTooltipDisplay : TooltipDisplay
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image rarityTag;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI descriptionText;



    public void Setup(Mod _mod)
    {

    }

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        SetGoalTransform(UIView.view.MouseViewPosition, Quaternion.identity, Vector3.one);
        UpdateTravel();
    }
}