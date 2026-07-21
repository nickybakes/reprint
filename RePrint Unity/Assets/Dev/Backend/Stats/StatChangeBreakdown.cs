using System.Collections.Generic;

public class StatChangeBreakdown
{
    public StatChangeAmounts abilityStatChanges;

    public List<StatChangeAmounts> modStatChanges;

    public StatChangeResults statsBefore;
    public StatChangeResults statsAfter;

    public StatChangeBreakdown(StatChangeAmounts _abilityStatChanges, List<StatChangeAmounts> _modStatChanges)
    {
        abilityStatChanges = _abilityStatChanges;
        modStatChanges = _modStatChanges;
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

    private void ApplyStatChanges(Character character)
    {
        int totalPhysicalDamageTaken = GetTotalStatChange(character, StatChange.PhysicalDamageTaken);

        character.ApplyPhysicalDamage(totalPhysicalDamageTaken);
    }

    public int GetTotalStatChange(Character character, StatChange stat)
    {
        int total = 0;

        total += abilityStatChanges.GetAmount(character, stat);

        foreach (StatChangeAmounts statChangeAmounts in modStatChanges)
        {
            total += statChangeAmounts.GetAmount(character, stat);
        }

        return total;
    }
}