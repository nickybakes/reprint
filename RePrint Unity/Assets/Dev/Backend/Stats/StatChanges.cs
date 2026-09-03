using System;

public class StatChanges
{
    public static int STAT_CHANGE_LENGTH = Enum.GetValues(typeof(StatChange)).Length;

    private float[] values;

    public StatChanges()
    {
        values = new float[STAT_CHANGE_LENGTH];
        for (int i = 0; i < values.Length; i++)
        {
            if (isMultiplicative((StatChange)i))
            {
                values[i] = 1;
            }
        }
    }

    public void StackAmount(StatChange stat, float amount)
    {
        if (isMultiplicative(stat))
        {
            values[(int)stat] *= amount;
        }
        else
        {
            values[(int)stat] += amount;
        }
    }

    private bool isMultiplicative(StatChange stat)
    {
        return stat == StatChange.StarterPhysicalDamageMultiplier || stat == StatChange.FinisherPhysicalDamageMultiplier || stat == StatChange.KineticDamageMultiplier;
    }

    public float GetAmount(StatChange stat)
    {
        return values[(int)stat];
    }
}

public enum StatChange
{
    StarterPhysicalDamageTaken,
    FinisherPhysicalDamageTaken,
    StarterPhysicalDamageMultiplier,
    FinisherPhysicalDamageMultiplier,
    KineticDamageTaken,
    KineticDamageMultiplier,
    HealthGained,
    DodgeTaken,
    DodgeGained,
    TurnPriorityGained,
    ChainGained,
    ChainSpent,
    ChainTaken,
    TempChainGained,
    APMaxIncrease,
    APMaxDecrease,
    CritChanceIncrease,
    CritChanceDecrease,
    HitAmountIncrease,
}