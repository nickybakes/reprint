using UnityEngine;

/// <summary>
/// A basic in-game UI component that has reference to its Rect Transform
/// </summary>
public class CharacterActionDisplay : TravelingDisplay
{
    private BattleController controller;

    private CharacterAction action;

    /// <summary>
    /// Setup basic travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
    }

    public void DisplayAction(CharacterAction _action, BattleController _controller)
    {
        action = _action;
        controller = _controller;
    }

    /// <summary>
    /// Function to call when the player submits (clicks) this card.
    /// </summary>
    public void SubmitAction()
    {
        controller.ActionSubmit(action);
    }
}