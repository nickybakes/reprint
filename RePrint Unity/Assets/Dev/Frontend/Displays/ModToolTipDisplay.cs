

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModTooltipDisplay : TooltipDisplay
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image rarityTag;
    [SerializeField] private TextMeshProUGUI rarityText;

    [SerializeField] private SubDescriptionGroup subDescriptionGroup;

    public void Setup(Mod _mod)
    {
        Color color = UIView.view.RarityDirectory.GetColor(_mod.Rarity);
        string rarityString = UIView.view.RarityDirectory.GetString(_mod.Rarity);

        iconImage.sprite = _mod.Profile.Icon;
        nameText.text = _mod.Profile.Name;

        iconImage.color = color;
        nameText.color = color;
        rarityTag.color = color;

        rarityText.text = rarityString;

        SubTagResult subTagResult = UIView.view.SubTagDirectory.GetAllSubTagResults(_mod.Profile.Description);

        subDescriptionGroup.DisplayDescriptions(subTagResult.replaceString, subTagResult.subDescriptions);

        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        MoveToMousePosition();
    }

    void Awake()
    {
        AwakeTooltip();
    }

    void Update()
    {
        MoveToMousePosition();
    }
}