using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityBehavior
{
    [SerializeField] private BetterEditorList<AbilityCondition> conditions;
    [SerializeField] private BetterEditorList<AbilityEffect> effects;

    public List<AbilityCondition> Conditions { get => conditions.List; }
    public List<AbilityEffect> Effects { get => effects.List; }

}