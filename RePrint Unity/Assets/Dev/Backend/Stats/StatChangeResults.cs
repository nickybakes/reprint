using System.Collections.Generic;

public class StatChangeResults
{
    public CharacterStats PlayerStats { get; private set; }

    public Dictionary<Character, CharacterStats> EnemyStats { get; private set; }
    public Dictionary<Character, CharacterStats> AllCharacterStats { get; private set; }

    public StatChangeResults(Character player, Team enemyTeam)
    {
        PlayerStats = new CharacterStats(player.Stats);
        EnemyStats = new Dictionary<Character, CharacterStats>();
        AllCharacterStats = new Dictionary<Character, CharacterStats>
        {
            { player, PlayerStats }
        };
        foreach (Character enemy in enemyTeam.Members)
        {
            CharacterStats enemyStats = new CharacterStats(enemy.Stats);
            EnemyStats.Add(enemy, enemyStats);
            AllCharacterStats.Add(enemy, enemyStats);

        }
    }
}