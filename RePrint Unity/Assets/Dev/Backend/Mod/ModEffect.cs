using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModEffect : Effect
{
    [SerializeField] private ModEffectType type;
    public ModEffectType Type { get => type; }

    [SerializeField] private ValueInput valueInput1;
    public ValueInput ValueInput1 { get => valueInput1; }

    [SerializeField] private BetterEditorList<Arithmetic> extraArithmetics1;
    public List<Arithmetic> ExtraArithmetics1 { get => extraArithmetics1.List; }


    [SerializeField] private MathType mathType;
    public MathType MathType { get => mathType; }

    [SerializeField] private StatChange statChange;
    public StatChange StatChange { get => statChange; }

    public override int GetAmount(GameValues gameValues, bool getMinimum = false, bool getMaximum = false)
    {
        int amount = ValueInput1.GetValue();

        if (getMinimum)
            amount = ValueInput1.GetMinValue();
        else if (getMaximum)
            amount = ValueInput1.GetMaxValue();

        if (ExtraArithmetics1 != null)
        {
            foreach (Arithmetic arithmetic in ExtraArithmetics1)
            {
                amount = arithmetic.CalculateSolution(amount, gameValues.GetIntGameValue(arithmetic.GameValueType));
            }
        }


        return amount;
    }


}

public enum ModEffectType
{
    DoDamage,
    StackStatChange,
    Heal,
    GainChain,


}