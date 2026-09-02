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

    [SerializeField] private ValueInput instances;
    public ValueInput Instances { get => instances; }

    [SerializeField] private BetterEditorList<Arithmetic> instancesArithmetics;
    public List<Arithmetic> InstancesArithmetics { get => instancesArithmetics.List; }

    [SerializeField] private BetterEditorList<Arithmetic> extraArithmetics;
    public List<Arithmetic> ExtraArithmetics { get => extraArithmetics.List; }

    [SerializeField] private BetterEditorList<AbilityEffect> extraEffects;
    public List<AbilityEffect> ExtraEffects { get => extraEffects.List; }

    [SerializeField] private bool dontAutoCountHits;
    public bool DontAutoCountHits { get => dontAutoCountHits; }

    [SerializeField] private ValueInput hitAmount;
    public ValueInput HitAmount { get => hitAmount; }

    [SerializeField] private bool dontAddCharacterToUniqueHitList;
    public bool DontAddCharacterToUniqueHitList { get => dontAddCharacterToUniqueHitList; }


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

    public int GetInstanceAmount(GameValues gameValues, bool getMinimum = false, bool getMaximum = false)
    {
        float amount = Instances.GetValue();

        if (getMinimum)
            amount = Instances.GetMinValue();
        else if (getMaximum)
            amount = Instances.GetMaxValue();

        foreach (Arithmetic arithmetic in InstancesArithmetics)
        {
            amount = arithmetic.CalculateSolution(amount, gameValues.GetIntGameValue(arithmetic.GameValueType));
        }

        return (int)amount;
    }

    public int GetHitAmount(GameValues gameValues, bool getMinimum = false, bool getMaximum = false)
    {
        float amount = HitAmount.GetValue();

        if (getMinimum)
            amount = HitAmount.GetMinValue();
        else if (getMaximum)
            amount = HitAmount.GetMaxValue();

        foreach (Arithmetic arithmetic in ExtraArithmetics)
        {
            amount = arithmetic.CalculateSolution(amount, gameValues.GetIntGameValue(arithmetic.GameValueType));
        }

        return (int)amount;
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