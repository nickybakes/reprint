using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Effect
{
    [SerializeField] protected BetterEditorList<EffectApplication> applicationModes;

    public List<EffectApplication> ApplicationModes { get => applicationModes.List; }

    [SerializeField] protected ValueInput occurrences;
    public ValueInput Occurrences { get => occurrences; }

    [SerializeField] protected BetterEditorList<Arithmetic> occurrencesArithmetics;
    public List<Arithmetic> OccurrencesArithmetics { get => occurrencesArithmetics.List; }

    public virtual float GetAmount(GameValues gameValues, bool getMinimum = false, bool getMaximum = false)
    {
        return 0;
    }

    public int GetOcurrences(GameValues gameValues, bool getMinimum = false, bool getMaximum = false)
    {
        float amount = Occurrences.GetValue();

        if (getMinimum)
            amount = Occurrences.GetMinValue();
        else if (getMaximum)
            amount = Occurrences.GetMaxValue();

        foreach (Arithmetic arithmetic in OccurrencesArithmetics)
        {
            amount = arithmetic.CalculateSolution(amount, gameValues.GetIntGameValue(arithmetic.GameValueType));
        }

        return (int)amount;
    }
}