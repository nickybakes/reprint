using System;
using UnityEngine;

[Serializable]
public class ModEffect
{
    [SerializeField] private ValueInput chainGainAmount;
    [SerializeField] private BetterEditorList<AbilityEffectModifier> modifiers;
}
