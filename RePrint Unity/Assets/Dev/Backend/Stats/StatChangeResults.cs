using System.Collections.Generic;

public class StatChangeResults
{
    public CharacterStats PlayerStats { get; private set; }

    public Dictionary<Character, CharacterStats> EnemyStats { get; private set; }

    public StatChangeResults(Character player, Team enemyTeam)
    {
        PlayerStats = new CharacterStats(player.Stats);
        EnemyStats = new Dictionary<Character, CharacterStats>();
        foreach (Character enemy in enemyTeam.Members)
        {
            EnemyStats.Add(enemy, new CharacterStats(enemy.Stats));
        }
    }
}