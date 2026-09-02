using System;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeBreakdown
{
    public StatChangeAmounts abilityStatChanges;

    public List<ModResult> modResults;

    public Character activator;

    public StatChangeResults statsBefore;
    public StatChangeResults statsAfter;

    public StatChangeBreakdown(StatChangeAmounts _abilityStatChanges, List<ModResult> _modResults, Character _activator)
    {
        activator = _activator;
        abilityStatChanges = _abilityStatChanges;
        modResults = _modResults;
    }

    public void ApplyStatChanges(Character player, Team enemyTeam)
    {
        statsBefore = new StatChangeResults(player, enemyTeam);

        ApplyStatChanges(player);

        foreach (Character character in enemyTeam.Members)
        {
            ApplyStatChanges(character);
        }

        statsAfter = new StatChangeResults(player, enemyTeam);
    }

    /// <summary>
    /// TODO:
    /// Implement all stat changes being applied to the character's stats
    /// </summary>
    /// <param name="character"></param>
    private void ApplyStatChanges(Character character)
    {
        int totalStarterPhysicalDamageTaken = (int)GetTotalStatChangeAdditive(character, StatChange.StarterPhysicalDamageTaken);
        float totalStarterPhysicalDamageMultiplier = GetTotalStatChangeMultiplicative(activator, StatChange.StarterPhysicalDamageMultiplier);
        character.ApplyPhysicalDamage(totalStarterPhysicalDamageTaken, totalStarterPhysicalDamageMultiplier);

        int totalFinisherPhysicalDamageTaken = (int)GetTotalStatChangeAdditive(character, StatChange.FinisherPhysicalDamageTaken);
        float totalFinisherPhysicalDamageMultiplier = GetTotalStatChangeMultiplicative(activator, StatChange.FinisherPhysicalDamageMultiplier);
        character.ApplyPhysicalDamage(totalFinisherPhysicalDamageTaken, totalFinisherPhysicalDamageMultiplier);

        int totalKineticDamageTaken = (int)GetTotalStatChangeAdditive(character, StatChange.KineticDamageTaken);
        float totalKineticDamageMultiplier = GetTotalStatChangeMultiplicative(activator, StatChange.KineticDamageMultiplier);
        character.ApplyPhysicalDamage(totalKineticDamageTaken, totalKineticDamageMultiplier);

        int totalDodgeGained = (int)GetTotalStatChangeAdditive(character, StatChange.DodgeGained);
        totalDodgeGained -= (int)GetTotalStatChangeAdditive(character, StatChange.DodgeTaken);
        character.ApplyDodge(totalDodgeGained);

        int totalChainGained = (int)GetTotalStatChangeAdditive(character, StatChange.ChainGained);
        totalChainGained -= (int)GetTotalStatChangeAdditive(character, StatChange.ChainSpent);
        totalChainGained -= (int)GetTotalStatChangeAdditive(character, StatChange.ChainTaken);
        character.ApplyChain(totalChainGained);

        int totalTempChainGained = (int)GetTotalStatChangeAdditive(character, StatChange.TempChainGained);
        character.Stats.TempChain += totalTempChainGained;

        int totalTurnPriorityGained = (int)GetTotalStatChangeAdditive(character, StatChange.TurnPriorityGained);
        character.Stats.TurnPriority += totalTurnPriorityGained;

        int totalAPMaxIncrease = (int)GetTotalStatChangeAdditive(character, StatChange.APMaxIncrease);
        totalAPMaxIncrease -= (int)GetTotalStatChangeAdditive(character, StatChange.APMaxDecrease);

        character.Stats.AbilityPointsMax = Math.Max(character.Stats.AbilityPointsMax + totalAPMaxIncrease, 0);

    }

    public float GetTotalStatChangeAdditive(Character character, StatChange stat)
    {
        float total = 0;

        if (abilityStatChanges != null)
            total += abilityStatChanges.GetTotalAmount(character, stat);

        foreach (ModResult modResult in modResults)
        {
            total += modResult.statChangeAmounts.GetTotalAmount(character, stat);
        }

        return total;
    }

    public float GetTotalStatChangeMultiplicative(Character character, StatChange stat, float startingValue = 1)
    {
        float total = startingValue;

        if (abilityStatChanges != null && abilityStatChanges.Changes.ContainsKey(character))
        {
            total *= abilityStatChanges.GetTotalAmount(character, stat);
        }

        foreach (ModResult modResult in modResults)
        {
            if (modResult.statChangeAmounts.Changes.ContainsKey(character))
            {
                total *= modResult.statChangeAmounts.GetTotalAmount(character, stat);
            }
        }

        return total;
    }

    public float GetTotalStatChangeEscalating(Character character, StatChange stat, bool subtractFromBase = false, float startingValue = 1, float baseValue = 0, float topValue = 1)
    {
        float total = startingValue;

        if (abilityStatChanges != null && abilityStatChanges.Changes.ContainsKey(character))
        {
            total *= topValue - abilityStatChanges.GetTotalAmount(character, stat);
        }

        foreach (ModResult modResult in modResults)
        {
            if (modResult.statChangeAmounts.Changes.ContainsKey(character))
            {
                total *= topValue - modResult.statChangeAmounts.GetTotalAmount(character, stat);
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