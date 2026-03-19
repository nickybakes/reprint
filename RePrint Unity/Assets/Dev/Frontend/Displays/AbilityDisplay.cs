using UnityEngine;

/// <summary>
/// A basic in-game UI component that has reference to its Rect Transform
/// </summary>
public class AbilityDisplay : TravelingDisplay
{
    private BattleController controller;

    private Ability ability;

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
    }

    /// <summary>
    /// Function to call when the player submits (clicks) this card.
    /// </summary>
    public void SubmitAbility()
    {
        controller.SubmitAbility(ability);
    }
}