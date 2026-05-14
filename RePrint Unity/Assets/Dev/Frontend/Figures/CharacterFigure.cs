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
    /// The currently displayed states. When a character's stats get updated, we can reference these old stats to
    /// do unique effects like playing specific HUD animations for losing or gaining health.
    /// </summary>
    protected CharacterStats currentStats;


    /// <summary>
    /// Sets up the travel data.
    /// </summary>
    void Awake()
    {
        SetupTravelingTransformData();
        state = FigureState.Idle;
    }

    public void Setup(Character character)
    {
        currentStats = new CharacterStats(character.Stats);
    }

    public void PlayAbilityAnimation(AnimationTrigger trigger, Action _finishAbilityAnimationAction, Action _returnToIdleAction, Action _updateStatsAction)
    {
        state = FigureState.Ability;
        finishAbilityAnimationAction = _finishAbilityAnimationAction;
        returnToIdleAction = _returnToIdleAction;
        updateStatsAction = _updateStatsAction;
        animator.SetTrigger(trigger.TriggerName);
    }

    public void UpdateStats(CharacterStats stats)
    {
        if (stats.Health < currentStats.Health)
        {
            if (stats.Health == 0)
            {
                animator.SetTrigger("Death");
            }
            else
            {
                animator.SetTrigger("Hurt");
            }
        }

        currentStats.Health = stats.Health;
        currentStats.HealthMax = stats.HealthMax;
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
