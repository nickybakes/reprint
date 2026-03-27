using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityEffect
{
    [SerializeField] private AbilityEffectType type;
    [SerializeField] private ValueInput valueInput;
    [SerializeField] private BetterEditorList<Arithmetic> extraArithmetics;

    public AbilityEffectType Type { get => type; }

    public ValueInput ValueInput { get => valueInput; }
    public List<Arithmetic> ExtraArithmetics { get => extraArithmetics.List; }

}

public enum AbilityEffectType
{
    DoDamage,
    GainDodge,
    GainChain,
    ApplyStatusEffect,
    CooldownAbility,
    RemoveAllChain
}