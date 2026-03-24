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
    }


    public void RefreshSequenceState(bool expanded, int overclock)
    {
        animator.SetInteger("Overclock", overclock);
        if (overclock > -1)
            rarityText.text = rarityStrings[overclock];

        if (currentOverclock != overclock)
        {
            currentOverclock = overclock;
            PlaySubmitAnimation();
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