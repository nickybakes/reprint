using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityEffect : Effect
{
    [SerializeField] private AbilityEffectType type;

    [SerializeField] private ValueInput valueInput;
    [SerializeField] private BetterEditorList<Arithmetic> extraArithmetics;

    public AbilityEffectType Type { get => type; }

    public ValueInput ValueInput { get => valueInput; }
    public List<Arithmetic> ExtraArithmetics { get => extraArithmetics.List; }


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
    SpendChain
}