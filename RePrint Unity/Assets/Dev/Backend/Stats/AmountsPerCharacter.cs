using System;
using System.Collections.Generic;

public class AmountsPerCharacter
{

    public Dictionary<Character, int> Amounts { get; private set; }

    public Dictionary<Character, int> Priorities { get; private set; }

    private Character player;

    private Team enemyTeam;

    public AmountsPerCharacter(Character _player, Team _enemyTeam)
    {
        player = _player;
        enemyTeam = _enemyTeam;
        Amounts = new Dictionary<Character, int>();
        Priorities = new Dictionary<Character, int>();
    }

    public void ApplyAmountsToCharacters(StatType statType)
    {
        foreach (Character character in Amounts.Keys)
        {
            switch (statType)
            {
                case StatType.PhysicalDamage:
                    character.ApplyPhysicalDamage(Amounts[character]);
                    break;
                case StatType.Dodge:
                    character.ApplyDodge(Amounts[character]);
                    break;
                case StatType.Chain:
                    character.ApplyChain(Amounts[character]);
                    break;
            }
        }
    }

    public void ApplyPrioritiesToCharacter(Character activator, StatType statType)
    {
        foreach (Character character in Priorities.Keys)
        {
            switch (statType)
            {
                case StatType.Dodge:
                    activator.ApplyDodgePriority(Priorities[character], character);
                    break;
            }
        }
    }

    public void NegateAmounts()
    {
        Dictionary<Character, int> newAmounts = new Dictionary<Character, int>();
        foreach (Character character in Amounts.Keys)
        {
            newAmounts.Add(character, Amounts[character] * -1);
        }
        Amounts = newAmounts;
    }

    public void NegatePriorities()
    {
        Dictionary<Character, int> newPriorities = new Dictionary<Character, int>();
        foreach (Character character in Priorities.Keys)
        {
            newPriorities.Add(character, Priorities[character] * -1);
        }
        Priorities = newPriorities;
    }

    public void AddAmountToCharacter(Character character, int amount, Character priorityCharacter = null, int priority = 0)
    {
        AddAmountToCharacter(character, amount);
        if (priorityCharacter != null && priority != 0)
        {
            AddPriorityToCharacter(priorityCharacter, priority);
        }
    }

    private void AddAmountToCharacter(Character character, int amount)
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

    private void AddPriorityToCharacter(Character character, int priority)
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

    public int GetTotalAmount()
    {
        int total = 0;

        foreach (int amount in Amounts.Values)
        {
            total += amount;
        }

        return total;
    }

    public int GetTotalAmountDoneToPlayer()
    {
        if (Amounts.ContainsKey(player))
        {
            return Amounts[player];
        }

        return 0;
    }

    public int GetTotalAmountDoneToEnemies()
    {
        int total = 0;

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