using System;
using System.Collections.Generic;
using UnityEngine;

public class Character
{

    public CharacterStats Stats { get; private set; }
    public CharacterStats AbilitySequencingStats { get; private set; }

    public InGameValues IncomingValues { get; private set; }

    public List<Ability> Abilities { get; private set; }

    public Dictionary<Character, int> DodgePriorities { get; private set; }

    public string Name { get; private set; }

    private int index;

    private bool isPlayerControlled;

    public bool IsPlayerControlled
    {
        get
        {
            return isPlayerControlled;
        }
    }

    public int Index
    {
        get
        {
            return index;
        }
    }

    public bool IsAlive
    {
        get
        {
            return Stats.Health > 0;
        }
    }

    public Character(CharacterData data)
    {
        Name = data.name;
        IncomingValues = new InGameValues();

        Stats = new CharacterStats();
        AbilitySequencingStats = new CharacterStats();

        Stats.HealthMax = data.maxHealth.GetValue();
        Stats.Health = Stats.HealthMax;

        Stats.AbilityPointsMax = data.abilityPointsMax.GetValue();
        Stats.AbilityPoints = Stats.AbilityPointsMax;

        Stats.Chain = 0;

        Abilities = new List<Ability>(data.abilities.Length);
        foreach (AbilityData abilityData in data.abilities)
        {
            Abilities.Add(new Ability(abilityData));
        }
    }

    public void RefillAbilityPoints()
    {
        Stats.AbilityPoints = Stats.AbilityPointsMax;
    }

    public void ResetDodge()
    {
        DodgePriorities = new Dictionary<Character, int>();
        Stats.Dodge = 0;
    }

    public void RefreshAbilitySequencingStats(AbilitySequence abilitySequence)
    {
        AbilitySequencingStats.CopyFrom(Stats);
        foreach (AbilitySelection abilitySelection in abilitySequence.Sequence)
        {
            AbilitySequencingStats.AbilityPoints -= abilitySelection.Ability.GetAbilityRules(abilitySelection.Overclock).APCost;
            if (AbilitySequencingStats.AbilityPoints < 0)
            {
                AbilitySequencingStats.AbilityPoints = 0;
            }
        }
    }

    public void ApplyPhysicalDamage(int damage)
    {
        //TODO: Use any resistances on the victim to lessen the damage

        // Use dodge first
        int tempDamage = damage;
        damage = Math.Max(0, damage - Stats.Dodge);
        Stats.Dodge = Math.Max(0, Stats.Dodge - tempDamage);

        if (damage > 0)
        {
            Stats.Health -= damage;
            Stats.Chain = 0;
        }
    }

    public void ApplyChain(int chain)
    {
        Stats.Chain += chain;
    }

    public void ApplyDodge(int dodge, Character target)
    {
        if (target != this)
        {
            if (DodgePriorities.ContainsKey(target))
            {
                DodgePriorities[target] += dodge;
            }
            else
            {
                DodgePriorities.Add(target, dodge);
            }
        }
        Stats.Dodge += dodge;
    }

    public void ApplyAbilitySequencingStats()
    {
        Stats.CopyFrom(AbilitySequencingStats);
    }

    public void RefreshIncomingValues(int numberOfEnemies)
    {
        IncomingValues.SetCalculatedChain(this);
        IncomingValues.SetNumberOfEnemies(numberOfEnemies);
    }
}