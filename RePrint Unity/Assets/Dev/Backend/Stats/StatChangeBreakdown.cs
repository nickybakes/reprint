using System;
using System.Collections.Generic;

public class StatChangeBreakdown
{
    public StatChangeAmounts abilityStatChanges;

    public List<ModResult> modResults;

    public StatChangeResults statsBefore;
    public StatChangeResults statsAfter;

    public StatChangeBreakdown(StatChangeAmounts _abilityStatChanges, List<ModResult> _modResults)
    {
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

        float totalStarterPhysicalDamageMultiplier = GetTotalStatChangeMultiplicative(character, StatChange.StarterPhysicalDamageMultiplier);

        float totalAllPhysicalDamageMultiplier = GetTotalStatChangeMultiplicative(character, StatChange.AllPhysicalDamageMultiplier);

        character.ApplyPhysicalDamage(totalStarterPhysicalDamageTaken, totalStarterPhysicalDamageMultiplier * totalAllPhysicalDamageMultiplier);

        int totalFinisherPhysicalDamageTaken = (int)GetTotalStatChangeAdditive(character, StatChange.FinisherPhysicalDamageTaken);

        float totalFinisherPhysicalDamageMultiplier = GetTotalStatChangeMultiplicative(character, StatChange.FinisherPhysicalDamageMultiplier);

        character.ApplyPhysicalDamage(totalFinisherPhysicalDamageTaken, totalFinisherPhysicalDamageMultiplier * totalAllPhysicalDamageMultiplier);

        int totalGenericPhysicalDamageTaken = (int)GetTotalStatChangeAdditive(character, StatChange.GenericPhysicalDamageTaken);

        character.ApplyPhysicalDamage(totalGenericPhysicalDamageTaken, totalAllPhysicalDamageMultiplier);

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
            total += abilityStatChanges.GetAmount(character, stat);

        foreach (ModResult modResult in modResults)
        {
            total += modResult.statChangeAmounts.GetAmount(character, stat);
        }

        return total;
    }

    public float GetTotalStatChangeMultiplicative(Character character, StatChange stat, float startingValue = 1)
    {
        float total = startingValue;

        if (abilityStatChanges != null && abilityStatChanges.Changes.ContainsKey(character))
        {
            total *= abilityStatChanges.GetAmount(character, stat);
        }

        foreach (ModResult modResult in modResults)
        {
            if (modResult.statChangeAmounts.Changes.ContainsKey(character))
            {
                total *= modResult.statChangeAmounts.GetAmount(character, stat);
            }
        }

        return total;
    }
}