using TMPro;
using UnityEngine;

public class AbilityDisplay : FloatingDisplay
{
    /// <summary>
    /// The animator attached to this Display.
    /// </summary>
    [SerializeField] private Animator animator;
    [SerializeField] private BetterButton button;


    [SerializeField] private TextDisplay nameText;
    [SerializeField] private TextDisplay descriptionText;
    [SerializeField] private TextDisplay rarityText;
    [SerializeField] private TextDisplay apCostText;

    [SerializeField] private string[] rarityStrings;

    [SerializeField] private AbilityStatDisplayGroup abilityStatDisplayGroup;
    [SerializeField] private SFXData hoverSound;

    private BattleController controller;

    private Ability ability;

    private bool currentlySelected;
    private bool currentlyExpanded;

    private int currentOverclock;

    /// <summary>
    /// Setup basic travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
    }

    public void PlayHoverSound()
    {
        SFXManager.sfx.Play(hoverSound);
    }

    public void DisplayAbility(Ability _ability, BattleController _controller)
    {
        ability = _ability;
        controller = _controller;

        nameText.SetTextNoBump(ability.Name);
        descriptionText.SetTextNoBump(ability.Description);
        if (apCostText)
            apCostText.SetTextNoBump(ability.GetAPCost().ToString());
    }


    public void RefreshSequenceState(bool expanded, int overclock, Character activator, int availableAP, int currentTempCost)
    {
        animator.SetInteger("Overclock", overclock);
        if (overclock > -1)
        {
            if (rarityText)
                rarityText.SetText(rarityStrings[overclock]);

            if (apCostText)
                apCostText.SetText(ability.GetAPCost(overclock).ToString());

            button.Interactable = true;

            if (abilityStatDisplayGroup)
                abilityStatDisplayGroup.Refresh(ability, overclock, activator);

            // if (overclock < Ability.MAX_OVERCLOCK && ability.GetAbilityRules(overclock + 1).APCost > availableAP)
            // {
            //     button.Interactable = false;
            // }
            // else
            // {
            //     button.Interactable = true;
            // }
        }
        else
        {
            if (apCostText)
                apCostText.SetText(ability.GetAPCost().ToString());

            if (abilityStatDisplayGroup)
                abilityStatDisplayGroup.Refresh(ability, 0, activator);

            if (ability.GetAPCost() > availableAP + currentTempCost)
            {
                button.Interactable = false;
            }
            else
            {
                button.Interactable = true;
            }
        }

        if (currentOverclock != overclock)
        {
            currentOverclock = overclock;
            // PlaySubmitAnimation();
        }

        if (expanded)
        {
            TryExpand();
        }
        else
        {
            TryCollapse();
        }
    }

    public void TryExpand()
    {
        if (!currentlyExpanded)
        {
            animator.SetTrigger("Expand");
            currentlyExpanded = true;
        }
    }

    public void TryCollapse()
    {
        if (currentlyExpanded && !currentlySelected)
        {
            animator.SetTrigger("Collapse");
        }
        if (currentlyExpanded)
        {
            currentlyExpanded = false;
        }
    }

    public void TrySelect()
    {
        if (!currentlyExpanded && !currentlySelected)
        {
            animator.SetTrigger("Expand");
        }
        currentlySelected = true;
    }

    public void TryDeselect()
    {
        if (!currentlyExpanded && currentlySelected)
        {
            animator.SetTrigger("Collapse");
        }
        currentlySelected = false;
    }

    private void PlaySubmitAnimation()
    {
        animator.ResetTrigger("Release");
        animator.SetTrigger("Press");
        animator.SetTrigger("Release");
    }

    /// <summary>
    /// Function to call when the player submits (clicks) this ability.
    /// </summary>
    public void SubmitAbility()
    {
        controller.SubmitAbility(ability);
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    void Update()
    {
        UpdateTravel();
    }
}