using System;
using System.Collections.Generic;

public class StatChangeAmounts
{
    public Dictionary<Character, StatChanges> Changes { get; private set; }
    public List<Dictionary<Character, StatChanges>> InstancesOfChanges { get; private set; }

    public List<Character> uniqueEnemiesHit;

    public int totalHits;

    private Character player;

    private Team enemyTeam;


    public StatChangeAmounts(Character _player, Team _enemyTeam)
    {
        player = _player;
        enemyTeam = _enemyTeam;
        Changes = new Dictionary<Character, StatChanges>();
        InstancesOfChanges = new List<Dictionary<Character, StatChanges>>
        {
            new Dictionary<Character, StatChanges>()
        };
        uniqueEnemiesHit = new List<Character>();
    }

    public void AddAmount(Character character, float amount, StatChange stat)
    {
        if (amount != 0)
        {
            if (Changes.ContainsKey(character))
            {
                Changes[character].StackAmount(stat, amount);
            }
            else
            {
                Changes.Add(character, new StatChanges());
                Changes[character].StackAmount(stat, amount);
            }

            Dictionary<Character, StatChanges> currentInstance = InstancesOfChanges[GetInstanceCount() - 1];

            if (currentInstance.ContainsKey(character))
            {
                currentInstance[character].StackAmount(stat, amount);
            }
            else
            {
                currentInstance.Add(character, new StatChanges());
                currentInstance[character].StackAmount(stat, amount);
            }
        }
    }

    public void StartNewInstance()
    {
        InstancesOfChanges.Add(new Dictionary<Character, StatChanges>());
    }

    // public void AddAmounts(Dictionary<Character, float> amounts, StatChange stat)
    // {
    //     foreach (Character character in amounts.Keys)
    //     {
    //         if (Changes.ContainsKey(character))
    //         {
    //             Changes[character].StackAmount(stat, amounts[character]);
    //         }
    //         else
    //         {
    //             Changes.Add(character, new StatChanges());
    //             Changes[character].StackAmount(stat, amounts[character]);
    //         }
    //     }
    // }

    public int GetInstanceCount()
    {
        return InstancesOfChanges.Count;
    }

    public float GetInstanceAmount(int instanceIndex, Character character, StatChange stat)
    {
        if (InstancesOfChanges[instanceIndex].ContainsKey(character))
        {
            return InstancesOfChanges[instanceIndex][character].GetAmount(stat);
        }

        return 0;
    }

    public float GetTotalAmount(Character character, StatChange stat)
    {
        if (Changes.ContainsKey(character))
        {
            return Changes[character].GetAmount(stat);
        }

        return 0;
    }
}