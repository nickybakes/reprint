using System;
using System.Collections.Generic;

public class AmountsPerCharacter
{

    public Dictionary<Character, float> Amounts { get; private set; }

    public Dictionary<Character, float> Priorities { get; private set; }

    private Character player;

    private Team enemyTeam;

    public AmountsPerCharacter(Character _player, Team _enemyTeam)
    {
        player = _player;
        enemyTeam = _enemyTeam;
        Amounts = new Dictionary<Character, float>();
        Priorities = new Dictionary<Character, float>();
    }

    public void NegateAmounts()
    {
        Dictionary<Character, float> newAmounts = new Dictionary<Character, float>();
        foreach (Character character in Amounts.Keys)
        {
            newAmounts.Add(character, Amounts[character] * -1);
        }
        Amounts = newAmounts;
    }

    public void NegatePriorities()
    {
        Dictionary<Character, float> newPriorities = new Dictionary<Character, float>();
        foreach (Character character in Priorities.Keys)
        {
            newPriorities.Add(character, Priorities[character] * -1);
        }
        Priorities = newPriorities;
    }

    public void AddAmountToCharacter(Character character, float amount, Character priorityCharacter = null, float priority = 0)
    {
        AddAmountToCharacter(character, amount);
        if (priorityCharacter != null && priority != 0)
        {
            AddPriorityToCharacter(priorityCharacter, priority);
        }
    }

    private void AddAmountToCharacter(Character character, float amount)
    {
        if (amount != 0)
        {
            if (Amounts.ContainsKey(character))
            {
                Amounts[character] += amount;
            }
            else
            {
                Amounts.Add(character, amount);
            }
        }
    }

    private void AddPriorityToCharacter(Character character, float priority)
    {
        if (priority != 0)
        {
            if (Priorities.ContainsKey(character))
            {
                Priorities[character] += priority;
            }
            else
            {
                Priorities.Add(character, priority);
            }
        }
    }

    public float GetTotalAmount()
    {
        float total = 0;

        foreach (float amount in Amounts.Values)
        {
            total += amount;
        }

        return total;
    }

    public float GetTotalAmountDoneToPlayer()
    {
        if (Amounts.ContainsKey(player))
        {
            return Amounts[player];
        }

        return 0;
    }

    public float GetTotalAmountDoneToEnemies()
    {
        float total = 0;

        foreach (Character enemy in enemyTeam.Members)
        {
            if (Amounts.ContainsKey(enemy))
            {
                total += Amounts[enemy];
            }
        }

        return total;
    }
}