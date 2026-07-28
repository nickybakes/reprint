using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityHit
{

    [SerializeField] private ValueInput amount;
    public ValueInput Amount { get => amount; }


    [SerializeField] private BetterEditorList<AbilityEffect> effects;
    public List<AbilityEffect> Effects { get => effects.List; }

    [SerializeField] private BetterEditorList<Arithmetic> extraArithmetics;
    public List<Arithmetic> ExtraArithmetics { get => extraArithmetics.List; }

    public int GetAmount(GameValues gameValues, bool getMinimum = false, bool getMaximum = false)
    {
        float total = amount.GetValue();

        if (getMinimum)
            total = amount.GetMinValue();
        else if (getMaximum)
            total = amount.GetMaxValue();

        foreach (Arithmetic arithmetic in ExtraArithmetics)
        {
            total = arithmetic.CalculateSolution(total, gameValues.GetIntGameValue(arithmetic.GameValueType));
        }

        return (int)total;
    }

}