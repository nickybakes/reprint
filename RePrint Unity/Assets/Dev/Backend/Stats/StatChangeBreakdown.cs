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
        int totalPhysicalDamageTaken = GetTotalStatChange(character, StatChange.PhysicalDamageTaken);

        character.ApplyPhysicalDamage(totalPhysicalDamageTaken);

        int totalDodgeGained = GetTotalStatChange(character, StatChange.DodgeGained);
        totalDodgeGained -= GetTotalStatChange(character, StatChange.DodgeTaken);
        character.ApplyDodge(totalDodgeGained);

        int totalChainGained = GetTotalStatChange(character, StatChange.ChainGained);
        totalChainGained -= GetTotalStatChange(character, StatChange.ChainSpent);
        character.ApplyChain(totalChainGained);

        int totalTurnPriorityGained = GetTotalStatChange(character, StatChange.TurnPriorityGained);
        character.Stats.TurnPriority += totalTurnPriorityGained;
    }

    public int GetTotalStatChange(Character character, StatChange stat)
    {
        int total = 0;

        total += abilityStatChanges.GetAmount(character, stat);

        foreach (ModResult modResult in modResults)
        {
            total += modResult.statChangeAmounts.GetAmount(character, stat);
        }

        return total;
    }
}