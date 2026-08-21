

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
        Color color = UIView.view.RarityDirectory.GetColor(_mod.Rarity);
        string rarityString = UIView.view.RarityDirectory.GetString(_mod.Rarity);

        iconImage.sprite = _mod.Profile.Icon;
        nameText.text = _mod.Profile.Name;

        iconImage.color = color;
        nameText.color = color;
        rarityTag.color = color;

        rarityText.text = rarityString;
        descriptionText.text = _mod.Profile.Description;

        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        MoveToMousePosition();
    }

    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
        MoveToMousePosition();
        gameObject.SetActive(false);
    }

    void Update()
    {
        MoveToMousePosition();
    }
}