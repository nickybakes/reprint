using System;
using System.Collections.Generic;
using UnityEngine;


public enum AbilityType
{
    Starter,
    Finisher,
    Utility,
    Charger,
}

public abstract class Ability
{
    public static int MAX_OVERCLOCK = 4;

    public AbilityProfile Profile { get { return baseData.Profile; } }

    public string Name { get { return baseData.Profile.Name; } }
    public string Description { get { return baseData.Profile.Description; } }
    public AbilityType Type { get { return baseData.Type; } }
    protected PlayerAbilityData baseData;

    protected List<List<AbilityBehavior>> behaviorsTable;

    public abstract int GetAPCost(int overclock = 0);
    public abstract bool TargetAllEnemies(int overclock = 0);
    public abstract bool CanTargetPlayer(int overclock = 0);
    public abstract bool CanTargetEnemies(int overclock = 0);

    public List<AbilityBehavior> GetAbilityBehaviors(int overclock = 0)
    {
        return behaviorsTable[overclock];
    }

    public AbilityStats GetAbilityStats(Character activator, int overclock = 0)
    {
        List<List<bool>> combinations = GetBehaviorCombinations(GetAbilityBehaviors(overclock));

        AbilityStats stats = new AbilityStats
        {
            MinPhysicalDamage = GetMinOrMaxStat(true, activator, overclock, combinations, StatType.PhysicalDamage),
            MaxPhysicalDamage = GetMinOrMaxStat(false, activator, overclock, combinations, StatType.PhysicalDamage),
            MinDodge = GetMinOrMaxStat(true, activator, overclock, combinations, StatType.Dodge),
            MaxDodge = GetMinOrMaxStat(false, activator, overclock, combinations, StatType.Dodge),
            MinChain = GetMinOrMaxStat(true, activator, overclock, combinations, StatType.Chain),
            MaxChain = GetMinOrMaxStat(false, activator, overclock, combinations, StatType.Chain),
        };

        return stats;
    }

    public List<AbilityEffect> GetAbilityEffects(List<AbilityBehavior> behaviors, List<bool> passingBehaviors)
    {
        List<AbilityEffect> effects = new List<AbilityEffect>();
        for (int i = 0; i < passingBehaviors.Count; i++)
        {
            if (passingBehaviors[i])
            {
                effects.AddRange(behaviors[i].Effects);
                if (behaviors[i].BreakOutIfConditionsAreTrue)
                {
                    return effects;
                }
            }
        }
        return effects;
    }

    protected int GetMinOrMaxStat(bool getMinimum, Character activator, int overclock, List<List<bool>> combinations, StatType type)
    {
        Character placeholderTarget = new DummyCharacter(activator.battleManager);
        int bestValue = int.MinValue;
        if (getMinimum)
            bestValue = int.MaxValue;

        for (int x = 0; x < combinations.Count; x++)
        {
            int currentValue = 0;
            List<bool> passingBehaviors = combinations[x];
            List<AbilityEffect> effects = GetAbilityEffects(GetAbilityBehaviors(overclock), passingBehaviors);

            if (effects.Count > 0)
            {
                StatChangeAmounts statChangeAmounts = StatCalculation.GetMinOrMaxStat(activator.battleManager, getMinimum, activator, placeholderTarget, effects);

                switch (type)
                {
                    case StatType.PhysicalDamage:
                        currentValue += (int)statChangeAmounts.GetAmount(placeholderTarget, StatChange.StarterPhysicalDamageTaken);
                        currentValue += (int)statChangeAmounts.GetAmount(placeholderTarget, StatChange.FinisherPhysicalDamageTaken);
                        break;
                    case StatType.Dodge:
                        currentValue += (int)statChangeAmounts.GetAmount(activator, StatChange.DodgeGained);
                        break;
                    case StatType.Chain:
                        currentValue += (int)statChangeAmounts.GetAmount(activator, StatChange.ChainGained);
                        currentValue -= (int)statChangeAmounts.GetAmount(activator, StatChange.ChainSpent);
                        break;
                }
                if (getMinimum)
                {
                    if (currentValue < bestValue)
                        bestValue = currentValue;
                }
                else
                {
                    if (currentValue > bestValue)
                        bestValue = currentValue;
                }
            }
        }

        if ((getMinimum && bestValue == int.MaxValue) || bestValue == int.MinValue)
            return 0;
        return bestValue;
    }

    public List<List<bool>> GetBehaviorCombinations(List<AbilityBehavior> behaviors)
    {
        int rows = behaviors.Count;
        int cols = (int)Math.Pow(2, rows);
        List<List<bool>> combinations = new List<List<bool>>(cols);

        for (int x = 0; x < cols; x++)
        {
            List<bool> row = new List<bool>(rows);

            for (int y = 0; y < rows; y++)
            {
                row.Add(x / (int)Math.Pow(2, rows - y - 1) % 2 == 0);
            }

            combinations.Add(row);
        }

        return combinations;
    }
}
