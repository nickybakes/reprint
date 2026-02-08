using TMPro;
using UnityEngine;

public class CharacterActionButton : RePrintButton
{

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Range(0.0f, 1.0f)] public float sizeTransition;

    private CharacterActionMenu menu;

    private RectTransform rect;

    public RectTransform GetRect()
    {
        return rect;
    }

    protected override void Awake()
    {
        base.Awake();
        rect = GetComponent<RectTransform>();
    }

    public void SetupActionButton(CharacterActionData data, int _index, CharacterActionMenu _menu)
    {
        index = _index;
        menu = _menu;

        nameText.text = data.actionName;

        descriptionText.text = data.description;
    }

    public void SetSelectedIndexInMenu()
    {
        menu.CurrentSelectedButtonIndex = index;
    }
}
