using System;
using UnityEngine;

public enum FigureState
{
    Idle,
    Ability
}

public class CharacterFigure : FloatingFigure
{

    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform positionBone;

    private FigureState state;

    private Action finishAbilityAnimationAction;
    private Action returnToIdleAction;
    private Action updateStatsAction;


    /// <summary>
    /// Sets up the travel data.
    /// </summary>
    void Awake()
    {
        SetupTravelingTransformData();
        state = FigureState.Idle;
    }

    public void PlayAnimation(AnimationTrigger trigger, Action _finishAbilityAnimationAction, Action _returnToIdleAction, Action _updateStatsAction)
    {
        state = FigureState.Ability;
        finishAbilityAnimationAction = _finishAbilityAnimationAction;
        returnToIdleAction = _returnToIdleAction;
        updateStatsAction = _updateStatsAction;
        animator.SetTrigger(trigger.TriggerName);
    }

    public void AnimEventFinishAbility()
    {
        if (finishAbilityAnimationAction != null)
        {
            finishAbilityAnimationAction.Invoke();
        }
    }

    public void AnimEventReturnToIdle()
    {
        if (returnToIdleAction != null)
        {
            returnToIdleAction.Invoke();
        }
    }

    public void AnimEventUpdateStats()
    {
        if (updateStatsAction != null)
        {
            updateStatsAction.Invoke();
        }
    }

    void Update()
    {
        if (state == FigureState.Idle)
        {
            UpdateTravel();
        }
        else if (state == FigureState.Ability)
        {
            // TODO: Read Locator bone position to move character based to target.
        }
    }
}
