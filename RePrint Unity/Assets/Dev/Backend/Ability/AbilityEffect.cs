using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityEffect
{
    [SerializeField] private AbilityEffectType type;
    [SerializeField] private BetterEditorList<AbilityEffectApplication> applicationModes;

    [SerializeField] private ValueInput valueInput;
    [SerializeField] private BetterEditorList<Arithmetic> extraArithmetics;

    public AbilityEffectType Type { get => type; }
    public List<AbilityEffectApplication> ApplicationModes { get => applicationModes.List; }

    public ValueInput ValueInput { get => valueInput; }
    public List<Arithmetic> ExtraArithmetics { get => extraArithmetics.List; }


    public int GetAmount(GameValues gameValues, bool getMinimum = false, bool getMaximum = false)
    {
        int amount = ValueInput.GetValue();

        if (getMinimum)
            amount = ValueInput.GetMinValue();
        else if (getMaximum)
            amount = ValueInput.GetMaxValue();

        foreach (Arithmetic arithmetic in ExtraArithmetics)
        {
            amount = arithmetic.CalculateSolution(amount, gameValues.GetInGameValue(arithmetic.GameValueType));
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