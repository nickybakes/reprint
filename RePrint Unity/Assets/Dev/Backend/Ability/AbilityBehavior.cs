using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityBehavior
{
    [SerializeField] private BetterEditorList<GameCondition> conditions;

    [SerializeField] private BetterEditorList<AbilityEffect> effects;
    [SerializeField] private bool breakOutIfConditionsAreTrue;

    public List<GameCondition> Conditions { get => conditions.List; }

    public List<AbilityEffect> Effects { get => effects.List; }
    public bool BreakOutIfConditionsAreTrue { get => breakOutIfConditionsAreTrue; }

}