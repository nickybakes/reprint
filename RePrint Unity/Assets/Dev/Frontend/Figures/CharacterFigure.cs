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
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected List<Material> glitchMaterials;

    [SerializeField] private AnimationCurve movementEase;

    [SerializeField] protected bool doesGlitchEffect;
    [SerializeField] protected float glitchCooldown = .5f;
    [SerializeField] protected float glitchMinimumTime = .15f;
    [SerializeField] protected float glitchMultiplier = 2;
    [SerializeField] protected float glitchSpeedThreshold = .001f;
    [SerializeField] protected List<VisualEffectAndTransform> visualEffects;

    private BattleView battleView;

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

    // private float lerpValue;
    // private float previousLerpValue;

    // [SerializeField, Header("Movement Error Removal")] protected float lerpDifferenceThreshold = 0;
    // [SerializeField] protected int bigJumpFrameDelay = 1;

    // private int bigJumpFrames = 0;

    private float currentPositionLerp;
    private float startPositionLerp;
    private float goalPositionLerp;
    private float currentTransitionTime;
    private float transitionTime;



    /// <summary>
    /// Sets up the travel data.
    /// </summary>
    void Awake()
    {
        SetupTravelingTransformData();
        state = FigureState.Idle;
    }

    public void Setup(Character character, BattleView _battleView)
    {
        battleView = _battleView;
        currentStats = new CharacterStats(character.Stats);
        animator.SetBool("Idling", true);

        foreach (VisualEffectAndTransform effect in visualEffects)
        {
            _battleView.VFXManager.CacheEffect(effect.VisualEffect);
        }
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
        Debug.Log(Time.frameCount + " - " + trigger.TriggerName);
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
        animator.SetBool("Idling", false);

        if (finishAbilityAnimationAction != null)
        {
            finishAbilityAnimationAction.Invoke();
        }
    }

    public void AnimEventReturnToIdle()
    {
        animator.SetBool("Idling", true);

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

    public void AnimEventCameraFocusEnemies()
    {
        battleView.CameraManager.FocusEnemies();
    }

    public void AnimEventCameraFocusDefault()
    {
        battleView.CameraManager.FocusDefault();
    }

    public void AnimEventVFX(int index)
    {
        battleView.VFXManager.PlayEffect(visualEffects[index].VisualEffect, visualEffects[index].Transform, visualEffects[index].StayParented);
    }

    public void AnimEventMoveCharacter(float[] floatArray)
    {
        startPositionLerp = currentPositionLerp;
        goalPositionLerp = floatArray[0];
        transitionTime = floatArray[1];
        currentTransitionTime = 0;

        if (transitionTime == 0)
        {
            currentPositionLerp = goalPositionLerp;
            currentTransitionTime = 1;
            transitionTime = 1;
        }
    }

    void Update()
    {
        UpdateTravel();

        currentTransitionTime += Time.deltaTime;

        float transitionLerp = Mathf.Clamp(currentTransitionTime / transitionTime, 0, 1);
        transitionLerp = movementEase.Evaluate(transitionLerp);

        currentPositionLerp = Mathf.Lerp(startPositionLerp, goalPositionLerp, transitionLerp);

        MoveToTarget();

        if (doesGlitchEffect)
        {
            SetGlitchEffect();
        }

        // if (positionBone)
        // {
        //     float newLerpValue = Math.Abs(positionBone.localPosition.x * 100);
        //     if (newLerpValue - previousLerpValue < lerpDifferenceThreshold)
        //     {
        //         bigJumpFrames = 0;
        //         // Big Jump detected
        //         Debug.Log(Time.frameCount + " - Difference: " + (newLerpValue - previousLerpValue));
        //     }

        //     if (bigJumpFrames >= bigJumpFrameDelay)
        //     {
        //         lerpValue = newLerpValue;
        //     }
        //     else
        //     {
        //         bigJumpFrames += 1;
        //     }

        //     MoveToTarget();
        //     SetGlitchEffect();

        //     previousLerpValue = newLerpValue;
        // }
    }

    protected void MoveToTarget()
    {
        idlePosition = GetGoalPosition();
        transform.position = Vector3.LerpUnclamped(idlePosition, targetAttackPosition, currentPositionLerp);

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
    }

    protected void SetGlitchEffect()
    {
        float glitchLerp = glitchCooldown - timeStoodStill;

        glitchLerp = Math.Clamp(glitchLerp * (1f / glitchCooldown) * glitchMultiplier, 0, 1);

        foreach (Material material in glitchMaterials)
        {
            material.SetFloat("_Glitch_Effect", glitchLerp);
        }
    }
}
