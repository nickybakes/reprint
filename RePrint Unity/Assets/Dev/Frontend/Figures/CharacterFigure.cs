using System;
using System.Collections.Generic;
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
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected List<Material> glitchMaterials;

    [SerializeField] protected float glitchCooldown = .5f;
    [SerializeField] protected float glitchMinimumTime = .15f;
    [SerializeField] protected float glitchMultiplier = 2;
    [SerializeField] protected float glitchSpeedThreshold = .001f;


    private FigureState state;

    private Action finishAbilityAnimationAction;
    private Action returnToIdleAction;
    private Action updateStatsAction;

    /// <summary>
    /// The currently displayed states. When a character's stats get updated, we can reference these old stats to
    /// do unique effects like playing specific HUD animations for losing or gaining health.
    /// </summary>
    protected CharacterStats currentStats;

    private Vector3 idlePosition;

    private Vector3 targetAttackPosition;

    private CharacterFigure currentTarget;

    private Vector3 previousPosition;

    private float timeStoodStill;


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

    public Vector3 GetAttackPoint()
    {
        if (attackPoint)
        {
            return attackPoint.position;
        }

        return transform.position;
    }

    public void PlayAbilityAnimation(AnimationTrigger trigger, Action _finishAbilityAnimationAction, Action _returnToIdleAction, Action _updateStatsAction, CharacterFigure target)
    {
        state = FigureState.Ability;
        currentTarget = target;
        targetAttackPosition = target.GetAttackPoint();
        finishAbilityAnimationAction = _finishAbilityAnimationAction;
        returnToIdleAction = _returnToIdleAction;
        updateStatsAction = _updateStatsAction;
        animator.SetTrigger(trigger.TriggerName);
    }

    public void UpdateStats(CharacterStats stats)
    {
        if (stats.Health < currentStats.Health)
        {
            if (stats.Health <= 0)
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
        UpdateTravel();

        if (positionBone)
        {
            float lerpValue = Math.Abs(positionBone.localPosition.x * 100);
            idlePosition = GetGoalPosition();
            transform.position = Vector3.LerpUnclamped(idlePosition, targetAttackPosition, lerpValue);

            float speed = Vector3.Distance(previousPosition, transform.position) / Time.deltaTime;

            if (speed < glitchSpeedThreshold)
            {
                timeStoodStill += Time.deltaTime;
            }
            else
            {
                timeStoodStill = -glitchMinimumTime;
            }
            previousPosition = transform.position;

            float glitchLerp = glitchCooldown - timeStoodStill;

            glitchLerp = Math.Clamp(glitchLerp * (1f / glitchCooldown) * glitchMultiplier, 0, 1);

            foreach (Material material in glitchMaterials)
            {
                material.SetFloat("_Glitch_Effect", glitchLerp);
            }
        }
    }
}
