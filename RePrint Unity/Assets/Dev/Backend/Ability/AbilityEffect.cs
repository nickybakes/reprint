using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityEffect
{
    [SerializeField] private AbilityEffectType type;
    [SerializeField] private ValueInput valueInput;
    [SerializeField] private AbilityEffectModifierList modifiers;

    public AbilityEffectType Type { get => type; }

    public ValueInput ValueInput { get => valueInput; }
    public List<AbilityEffectModifier> Modifiers { get => modifiers.Modifiers; }

}

public enum AbilityEffectType
{
    DoDamage,
    GainDodge,
    GainChain,
    ApplyStatusEffect,
    CooldownAbility
}