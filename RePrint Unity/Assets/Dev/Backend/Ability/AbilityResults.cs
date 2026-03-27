using System.Collections.Generic;

public class AbilityResults
{

    public CharacterStats PlayerStatsAfter { get; private set; }

    public Dictionary<Character, CharacterStats> EnemyStatsAfter { get; private set; }

    public AbilityResults(Character player, Team enemyTeam)
    {
        PlayerStatsAfter = new CharacterStats(player.Stats);
        EnemyStatsAfter = new Dictionary<Character, CharacterStats>();
        foreach (Character enemy in enemyTeam.Members)
        {
            EnemyStatsAfter.Add(enemy, new CharacterStats(enemy.Stats));
        }
    }
}