using TMPro;
using UnityEngine;

public class AbilityDisplay : FloatingDisplay
{
    /// <summary>
    /// The animator attached to this Display.
    /// </summary>
    [SerializeField] private Animator animator;
    [SerializeField] private BetterButton button;


    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI apCostText;

    [SerializeField] private string[] rarityStrings;

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

    public void DisplayAbility(Ability _ability, BattleController _controller)
    {
        ability = _ability;
        controller = _controller;

        nameText.text = ability.Name;
        descriptionText.text = ability.Description;
        apCostText.text = ability.GetAbilityRules(0).APCost.ToString();
    }


    public void RefreshSequenceState(bool expanded, int overclock, int availableAP, int currentTempCost)
    {
        animator.SetInteger("Overclock", overclock);
        if (overclock > -1)
        {
            rarityText.text = rarityStrings[overclock];
            apCostText.text = ability.GetAbilityRules(overclock).APCost.ToString();

            button.Interactable = true;

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
            apCostText.text = ability.GetAbilityRules(0).APCost.ToString();

            if (ability.GetAbilityRules(0).APCost > availableAP + currentTempCost)
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
        if (currentlyExpanded)
        {
            animator.SetTrigger("Collapse");
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

    void Update()
    {
        UpdateTravel();
    }
}