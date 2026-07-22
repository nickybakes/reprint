using System;
using System.Collections.Generic;

public class StatChangeAmounts
{
    public Dictionary<Character, StatChanges> Changes { get; private set; }

    private Character player;

    private Team enemyTeam;


    public StatChangeAmounts(Character _player, Team _enemyTeam)
    {
        player = _player;
        enemyTeam = _enemyTeam;
        Changes = new Dictionary<Character, StatChanges>();
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

    // public void ApplyAmountsToCharacters(StatType statType)
    // {
    //     foreach (Character character in Amounts.Keys)
    //     {
    //         switch (statType)
    //         {
    //             case StatType.PhysicalDamage:
    //                 character.ApplyPhysicalDamage(Amounts[character]);
    //                 break;
    //             case StatType.Dodge:
    //                 character.ApplyDodge(Amounts[character]);
    //                 break;
    //             case StatType.Chain:
    //                 character.ApplyChain(Amounts[character]);
    //                 break;
    //         }
    //     }
    // }

    // public void AddAmountToCharacter(Character character, int amount, Character priorityCharacter = null, int priority = 0)
    // {
    //     AddAmountToCharacter(character, amount);
    //     if (priorityCharacter != null && priority != 0)
    //     {
    //         AddPriorityToCharacter(priorityCharacter, priority);
    //     }
    // }

    // private void AddAmountToCharacter(Character character, int amount)
    // {
    //     if (amount != 0)
    //     {
    //         if (Amounts.ContainsKey(character))
    //         {
    //             Amounts[character] += amount;
    //         }
    //         else
    //         {
    //             Amounts.Add(character, amount);
    //         }
    //     }
    // }

    // public int GetTotalAmount()
    // {
    //     int total = 0;

    //     foreach (int amount in Amounts.Values)
    //     {
    //         total += amount;
    //     }

    //     return total;
    // }

    // public int GetTotalAmountDoneToPlayer()
    // {
    //     if (Amounts.ContainsKey(player))
    //     {
    //         return Amounts[player];
    //     }

    //     return 0;
    // }

    // public int GetTotalAmountDoneToEnemies()
    // {
    //     int total = 0;

    //     foreach (Character enemy in enemyTeam.Members)
    //     {
    //         if (Amounts.ContainsKey(enemy))
    //         {
    //             total += Amounts[enemy];
    //         }
    //     }

    //     return total;
    // }
}