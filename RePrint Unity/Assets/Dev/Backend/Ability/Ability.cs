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

[Serializable]
public class Ability
{
    public static int MAX_OVERCLOCK = 4;

    public string Name { get { return baseData.Name; } }
    public string Description { get { return baseData.Description; } }
    public AbilityType Type { get { return baseData.Type; } }

    private AbilityData baseData;
    private List<List<AbilityBehavior>> abilityBehaviorsForEachOverclock;
    private List<AbilityRules> abilityRulesForEachOverclock;

    public Ability(AbilityData data)
    {
        baseData = data;

        abilityRulesForEachOverclock = new List<AbilityRules>(MAX_OVERCLOCK);

        abilityBehaviorsForEachOverclock = new List<List<AbilityBehavior>>(MAX_OVERCLOCK)
        {
            baseData.AbilityOverclock0Behaviors.List,
            baseData.AbilityOverclock1Behaviors.List,
            baseData.AbilityOverclock2Behaviors.List,
            baseData.AbilityOverclock3Behaviors.List,
            baseData.AbilityOverclock4Behaviors.List
        };

        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock0);
        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock1);
        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock2);
        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock3);
        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock4);
    }

    public AbilityRules GetAbilityRules(int overclock)
    {
        return abilityRulesForEachOverclock[overclock];
    }

    public List<AbilityBehavior> GetAbilityBehaviors(int overclock)
    {
        return abilityBehaviorsForEachOverclock[overclock];
    }

    public List<AbilityEffect> GetAbilityEffects(int overclock, List<bool> passingBehaviors)
    {
        List<AbilityEffect> effects = new List<AbilityEffect>();
        List<AbilityBehavior> behaviors = GetAbilityBehaviors(overclock);
        for (int i = 0; i < passingBehaviors.Count; i++)
        {
            //TODO: Check if behavior breaks if passing. If so, break out of the loop.
            if (passingBehaviors[i])
            {
                effects.AddRange(behaviors[i].Effects);
            }
        }
        return effects;
    }

    public AbilityStats GetAbilityStats(int overclock, Character activator)
    {
        List<List<bool>> combinations = GetBehaviorCombinations(overclock);

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

    private int GetMinOrMaxStat(bool getMinimum, Character activator, int overclock, List<List<bool>> combinations, StatType type)
    {
        int bestValue = int.MinValue;
        if (getMinimum)
            bestValue = int.MaxValue;

        for (int x = 0; x < combinations.Count; x++)
        {
            int currentValue = 0;
            List<bool> passingBehaviors = combinations[x];
            List<AbilityEffect> effects = GetAbilityEffects(overclock, passingBehaviors);

            if (effects.Count > 0)
            {
                currentValue += StatCalculation.GetMinOrMaxStat(getMinimum, activator, effects, type);

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

    public List<List<bool>> GetBehaviorCombinations(int overclock)
    {
        List<AbilityBehavior> behaviors = abilityBehaviorsForEachOverclock[overclock];
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
