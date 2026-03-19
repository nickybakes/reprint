using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityEffect
{
    public AbilityEffectType type;
    public ValueInput valueInput;
    public AbilityEffectModifierList modifiers;
}

public enum AbilityEffectType
{
    DoDamage,
    GainDodge,
    GainChain,
    ApplyStatusEffect,
    CooldownAbility
}