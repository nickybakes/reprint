using UnityEngine;

/// <summary>
/// A basic in-game UI component that has reference to its Rect Transform
/// </summary>
public class CharacterTargetDisplay : TravelingDisplay
{
    private BattleController controller;

    private Character target;

    /// <summary>
    /// Setup basic travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
    }

    public void DisplayTarget(Character _target, BattleController _controller)
    {
        target = _target;
        controller = _controller;
    }

    /// <summary>
    /// Function to call when the player submits (clicks) this card.
    /// </summary>
    public void SubmitTarget()
    {
        controller.TargetSubmit(target);
    }
}