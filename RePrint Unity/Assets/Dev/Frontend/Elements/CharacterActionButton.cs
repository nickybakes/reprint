using System;
using TMPro;
using UnityEngine;

public class CharacterActionButton : BetterButton
{

    /// <summary>
    /// Temporary storage of these strings, should be changed later to a better solution for localization n stuff
    /// </summary>
    private String[] rarityStrings = { "Common", "Uncommon", "Rare", "Ethereal", "Mythical" };

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI rarityText;
    public Transform actionStatsParent;

    [Range(0.0f, 1.0f)] public float sizeTransition;

    private CharacterActionMenu menu;

    private RectTransform rect;

    [HideInInspector] public bool isCurrentlyActiveAction;

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

        nameText.text = data.name;

        descriptionText.text = data.description;
    }

    public void RefreshButtonState(bool activated, int overclock)
    {
        animator.SetInteger("Overclock", overclock);
        if (overclock > -1)
            rarityText.text = rarityStrings[overclock];

        isCurrentlyActiveAction = activated;
        if (activated)
        {
            ResetAnimationTrigger("Release");
            SetAnimationTrigger("Press");
            SetAnimationTrigger("Release");
        }
        else
        {
            if (isPointerInside)
                Select();
            else
            {
                SetAnimationTrigger("Deselect");
                hasSelection = false;
            }
        }
    }

    public override void OnDeselected()
    {
        if (!isCurrentlyActiveAction)
        {
            base.OnDeselected();
        }
    }

    public void SubmitCharacterActionButton()
    {
        menu.SubmitCharacterActionButton(index);
    }

    public void SetSelectedIndexInMenu()
    {
        menu.CurrentSelectedButtonIndex = index;
    }
}
