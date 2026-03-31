using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{

    public CharacterStats AbilitySequencingStats { get; private set; }

    public List<PlayerAbility> Abilities { get; private set; }

    public Dictionary<Character, int> DodgePriorities { get; private set; }

    public PlayerCharacter(CharacterData data) : base()
    {
        AbilitySequencingStats = new CharacterStats(this);

        Stats.HealthMax = data.maxHealth.GetValue();
        Stats.Health = Stats.HealthMax;

        Stats.AbilityPointsMax = data.abilityPointsMax.GetValue();
        Stats.AbilityPoints = Stats.AbilityPointsMax;

        Stats.Chain = 0;

        Abilities = new List<PlayerAbility>(data.abilities.Length);
        foreach (PlayerAbilityData abilityData in data.abilities)
        {
            Abilities.Add(new PlayerAbility(abilityData));
        }
    }

    public override void ApplyDodge(int dodge, Character target)
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
        base.ApplyDodge(dodge, target);
    }

    public void RefreshAbilitySequencingStats(AbilitySequence abilitySequence)
    {
        AbilitySequencingStats.CopyFrom(Stats);
        foreach (AbilitySelection abilitySelection in abilitySequence.Sequence)
        {
            AbilitySequencingStats.AbilityPoints -= abilitySelection.Ability.GetAPCost(abilitySelection.Overclock);
            if (AbilitySequencingStats.AbilityPoints < 0)
            {
                AbilitySequencingStats.AbilityPoints = 0;
            }
        }
    }

    public void ApplyAbilitySequencingStats()
    {
        Stats.CopyFrom(AbilitySequencingStats);
    }

    public override void ResetForTurn()
    {
        Stats.AbilityPoints = Stats.AbilityPointsMax;
        DodgePriorities = new Dictionary<Character, int>();
        Stats.Dodge = 0;
    }
}