using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{

    public CharacterStats TurnStatsStorage { get; private set; }

    public List<PlayerAbility> Abilities { get; private set; }

    public PlayerCharacter(CharacterData data, ModData[] equippedMods) : base()
    {

        Profile = data.Profile;

        TurnStatsStorage = new CharacterStats(this);

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
        Stats.CopyFrom(TurnStatsStorage);

        // TODO check mods to change stats

        foreach (AbilitySelection abilitySelection in abilitySequence.Sequence)
        {
            Stats.AbilityPoints -= abilitySelection.Ability.GetAPCost(abilitySelection.Overclock);
            if (Stats.AbilityPoints < 0)
            {
                Stats.AbilityPoints = 0;
            }
        }
    }

    public override void ResetForTurn()
    {
        Stats.AbilityPoints = Stats.AbilityPointsMax;
        Stats.Dodge = 0;
        TurnStatsStorage.CopyFrom(Stats);
    }
}