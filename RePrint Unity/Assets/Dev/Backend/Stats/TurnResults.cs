

using System.Collections.Generic;

public class TurnResults
{
    public PlayerDoAbilitySequence playerDoAbilitySequence;

    public List<EnemyDoAbility> enemyDoAbilities;

    public StatChangeResults statsBefore;
    public StatChangeResults statsAfter;

    public TurnResults(Character player, Team enemyTeam)
    {
        statsBefore = new StatChangeResults(player, enemyTeam);
    }

    public void CalculateStatsAfter(Character player, Team enemyTeam)
    {
        statsAfter = new StatChangeResults(player, enemyTeam);
    }

    public int GetStatDifference(Character character, CharacterStat stat)
    {
        CharacterStats characterStatsBefore;
        CharacterStats characterStatsAfter;
        int beforeAmount = 0;
        int afterAmount = 0;

        if (statsBefore.AllCharacterStats.ContainsKey(character))
        {
            characterStatsBefore = statsBefore.AllCharacterStats[character];
        }
        else
        {
            characterStatsBefore = character.Stats;
        }

        if (statsAfter != null && statsAfter.AllCharacterStats.ContainsKey(character))
        {
            characterStatsAfter = statsAfter.AllCharacterStats[character];
        }
        else
        {
            characterStatsAfter = character.Stats;
        }

        switch (stat)
        {
            case CharacterStat.Health:
                beforeAmount = characterStatsBefore.Health;
                afterAmount = characterStatsAfter.Health;
                break;
        }


        return afterAmount - beforeAmount;
    }
}