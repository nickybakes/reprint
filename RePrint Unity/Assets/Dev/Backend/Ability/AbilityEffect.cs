using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityEffect : Effect
{
    [SerializeField] private AbilityEffectType type;
    public AbilityEffectType Type { get => type; }

    [SerializeField] private ValueInput valueInput;
    public ValueInput ValueInput { get => valueInput; }

    [SerializeField] private BetterEditorList<Arithmetic> extraArithmetics;
    public List<Arithmetic> ExtraArithmetics { get => extraArithmetics.List; }

    [SerializeField] private BetterEditorList<AbilityEffect> extraEffects;
    public List<AbilityEffect> ExtraEffects { get => extraEffects.List; }

    [SerializeField] private bool dontAutoCountHits;
    public bool DontAutoCountHits { get => dontAutoCountHits; }

    [SerializeField] private ValueInput hitAmount;
    public ValueInput HitAmount { get => hitAmount; }


    public override float GetAmount(GameValues gameValues, bool getMinimum = false, bool getMaximum = false)
    {
        float amount = ValueInput.GetValue();

        if (getMinimum)
            amount = ValueInput.GetMinValue();
        else if (getMaximum)
            amount = ValueInput.GetMaxValue();

        foreach (Arithmetic arithmetic in ExtraArithmetics)
        {
            amount = arithmetic.CalculateSolution(amount, gameValues.GetIntGameValue(arithmetic.GameValueType));
        }

        return amount;
    }

}

public enum AbilityEffectType
{
    DoDamage,
    GainDodge,
    GainChain,
    ApplyStatusEffect,
    CooldownAbility,
    SpendChain,
    CountHits,
}