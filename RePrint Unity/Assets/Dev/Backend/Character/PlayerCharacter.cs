using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{

    public CharacterStats AbilitySequencingStats { get; private set; }

    public List<PlayerAbility> Abilities { get; private set; }

    public PlayerCharacter(CharacterData data, ModData[] equippedMods) : base()
    {

        Profile = data.Profile;

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

        if (equippedMods == null)
        {
            Mods = new List<Mod>();
        }
        else
        {
            Mods = new List<Mod>(equippedMods.Length);
            foreach (ModData modData in equippedMods)
            {
                Mods.Add(new Mod(modData));
            }
        }
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
        Stats.Dodge = 0;
    }
}