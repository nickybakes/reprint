using System;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeBreakdown
{
    public StatChangeAmounts abilityStatChanges;

    public List<ModResult> modResults;

    public Character activator;

    public StatChangeResults statsBefore;

    public List<StatChangeResults> statChangeSteps;

    public StatChangeResults statsAfter;

    private int instanceCount;

    public StatChangeBreakdown(StatChangeAmounts _abilityStatChanges, List<ModResult> _modResults, Character _activator)
    {
        activator = _activator;
        abilityStatChanges = _abilityStatChanges;
        modResults = _modResults;
    }

    public void ApplyStatChanges(Character player, Team enemyTeam)
    {
        statsBefore = new StatChangeResults(player, enemyTeam);

        statChangeSteps = new List<StatChangeResults>();

        instanceCount = 0;
        if (abilityStatChanges != null)
            instanceCount = abilityStatChanges.GetInstanceCount();

        for (int i = 0; i < instanceCount; i++)
        {
            ApplyStatChanges(player, i);

            foreach (Character character in enemyTeam.Members)
            {
                ApplyStatChanges(character, i);
            }

            statChangeSteps.Add(new StatChangeResults(player, enemyTeam));
        }

        statsAfter = new StatChangeResults(player, enemyTeam);
    }

    /// <summary>
    /// TODO:
    /// Implement all stat changes being applied to the character's stats
    /// </summary>
    /// <param name="character"></param>
    private void ApplyStatChanges(Character character, int instanceIndex)
    {
        float totalStarterPhysicalDamageMultiplier = GetTotalStatChangeMultiplicative(activator, StatChange.StarterPhysicalDamageMultiplier, instanceIndex);
        float totalFinisherPhysicalDamageMultiplier = GetTotalStatChangeMultiplicative(activator, StatChange.FinisherPhysicalDamageMultiplier, instanceIndex);

        float critChance = activator.BaseCritChance + GetTotalStatChangeAdditive(activator, StatChange.CritChanceIncrease, instanceIndex) - GetTotalStatChangeAdditive(activator, StatChange.CritChanceDecrease, instanceIndex);

        bool isCrit = UnityEngine.Random.Range(0f, 100f) < critChance;

        float critMultiplier = activator.CritDamageMultiplier;

        int totalStarterPhysicalDamageTaken = (int)GetTotalStatChangeAdditive(character, StatChange.StarterPhysicalDamageTaken, instanceIndex);
        character.ApplyPhysicalDamage(totalStarterPhysicalDamageTaken, totalStarterPhysicalDamageMultiplier, critMultiplier, isCrit);

        int totalFinisherPhysicalDamageTaken = (int)GetTotalStatChangeAdditive(character, StatChange.FinisherPhysicalDamageTaken, instanceIndex);
        character.ApplyPhysicalDamage(totalFinisherPhysicalDamageTaken, totalFinisherPhysicalDamageMultiplier, critMultiplier, isCrit);

        int totalKineticDamageTaken = (int)GetTotalStatChangeAdditive(character, StatChange.KineticDamageTaken, instanceIndex);
        float totalKineticDamageMultiplier = GetTotalStatChangeMultiplicative(activator, StatChange.KineticDamageMultiplier, instanceIndex);
        character.ApplyPhysicalDamage(totalKineticDamageTaken, totalKineticDamageMultiplier, critMultiplier, false);

        int totalDodgeGained = (int)GetTotalStatChangeAdditive(character, StatChange.DodgeGained, instanceIndex);
        totalDodgeGained -= (int)GetTotalStatChangeAdditive(character, StatChange.DodgeTaken, instanceIndex);
        character.ApplyDodge(totalDodgeGained);

        int totalChainGained = (int)GetTotalStatChangeAdditive(character, StatChange.ChainGained, instanceIndex);
        totalChainGained -= (int)GetTotalStatChangeAdditive(character, StatChange.ChainSpent, instanceIndex);
        totalChainGained -= (int)GetTotalStatChangeAdditive(character, StatChange.ChainTaken, instanceIndex);
        character.ApplyChain(totalChainGained);

        int totalTempChainGained = (int)GetTotalStatChangeAdditive(character, StatChange.TempChainGained, instanceIndex);
        character.Stats.TempChain += totalTempChainGained;

        int totalTurnPriorityGained = (int)GetTotalStatChangeAdditive(character, StatChange.TurnPriorityGained, instanceIndex);
        character.Stats.TurnPriority += totalTurnPriorityGained;

        int totalAPMaxIncrease = (int)GetTotalStatChangeAdditive(character, StatChange.APMaxIncrease, instanceIndex);
        totalAPMaxIncrease -= (int)GetTotalStatChangeAdditive(character, StatChange.APMaxDecrease, instanceIndex);
        character.Stats.AbilityPointsMax = Math.Max(character.Stats.AbilityPointsMax + totalAPMaxIncrease, 0);
    }

    public float GetTotalStatChangeAdditive(Character character, StatChange stat, int instanceIndex)
    {
        float total = 0;

        if (abilityStatChanges != null)
            total += abilityStatChanges.GetTotalAmount(character, stat, instanceIndex);

        foreach (ModResult modResult in modResults)
        {
            total += modResult.statChangeAmounts.GetTotalAmount(character, stat, instanceIndex);
        }

        return total;
    }

    public float GetTotalStatChangeMultiplicative(Character character, StatChange stat, int instanceIndex, float startingValue = 1)
    {
        float total = startingValue;

        if (abilityStatChanges != null && abilityStatChanges.Changes.ContainsKey(character))
        {
            total *= abilityStatChanges.GetTotalAmount(character, stat, instanceIndex);
        }

        foreach (ModResult modResult in modResults)
        {
            if (modResult.statChangeAmounts.Changes.ContainsKey(character))
            {
                total *= modResult.statChangeAmounts.GetTotalAmount(character, stat, instanceIndex);
            }
        }

        return total;
    }

    public float GetTotalStatChangeEscalating(Character character, StatChange stat, int instanceIndex, bool subtractFromBase = false, float startingValue = 1, float baseValue = 0, float topValue = 1)
    {
        float total = startingValue;

        if (abilityStatChanges != null && abilityStatChanges.Changes.ContainsKey(character))
        {
            total *= topValue - abilityStatChanges.GetTotalAmount(character, stat, instanceIndex);
        }

        foreach (ModResult modResult in modResults)
        {
            if (modResult.statChangeAmounts.Changes.ContainsKey(character))
            {
                total *= topValue - modResult.statChangeAmounts.GetTotalAmount(character, stat, instanceIndex);
            }
        }

        if (subtractFromBase)
        {
            return baseValue - total;
        }
        else
        {
            return baseValue + total;
        }
    }
}