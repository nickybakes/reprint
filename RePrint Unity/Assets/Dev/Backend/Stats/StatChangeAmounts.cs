using System;
using System.Collections.Generic;

public class StatChangeAmounts
{
    public Dictionary<Character, StatChanges> Changes { get; private set; }

    public List<Character> uniqueEnemiesHit;

    public int totalHits;

    private Character player;

    private Team enemyTeam;


    public StatChangeAmounts(Character _player, Team _enemyTeam)
    {
        player = _player;
        enemyTeam = _enemyTeam;
        Changes = new Dictionary<Character, StatChanges>();
        uniqueEnemiesHit = new List<Character>();
    }

    public void AddAmount(Character character, float amount, StatChange stat)
    {
        if (amount != 0)
        {
            if (Changes.ContainsKey(character))
            {
                Changes[character].AddAmount(stat, amount);
            }
            else
            {
                Changes.Add(character, new StatChanges());
                Changes[character].AddAmount(stat, amount);
            }
        }
    }

    public void AddAmounts(Dictionary<Character, float> amounts, StatChange stat)
    {
        foreach (Character character in amounts.Keys)
        {
            if (Changes.ContainsKey(character))
            {
                Changes[character].AddAmount(stat, amounts[character]);
            }
            else
            {
                Changes.Add(character, new StatChanges());
                Changes[character].AddAmount(stat, amounts[character]);
            }
        }
    }

    public float GetAmount(Character character, StatChange stat)
    {
        if (Changes.ContainsKey(character))
        {
            return Changes[character].GetAmount(stat);
        }

        return 0;
    }
}