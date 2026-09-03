using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{

    public List<PlayerAbility> Abilities { get; private set; }

    public PlayerCharacter(CharacterData data, ModData[] equippedMods) : base()
    {

        Profile = data.Profile;

        CritDamageMultiplier = data.critDamageMultiplier.GetValue();
        BaseCritChance = data.baseCritChance.GetValue();

        Stats.HealthMax = (int)data.maxHealth.GetValue();
        Stats.Health = Stats.HealthMax;

        Stats.AbilityPointsMax = (int)data.abilityPointsMax.GetValue();
        Stats.AbilityPoints = Stats.AbilityPointsMax;

        BaseMaxAP = Stats.AbilityPointsMax;

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

    public void RefreshAbilitySequencingStats(AbilitySequence abilitySequence, BattleManager battleManager)
    {
        RestoreTurnStats();
        battleManager.EnemyTeam.RestoreTurnStats();

        battleManager.DoPlayerMods(GameEvent.OnRefreshAbilitySequenceStats);

        Stats.AbilityPoints = Stats.AbilityPointsMax;
        // Calculate current AP points
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
        Stats.AbilityPointsMax = BaseMaxAP;
        Stats.Dodge = 0;
        CurrentCombo = 0;
        TotalCombo = 0;
        UniqueCharactersHitThisTurn = new List<Character>();
        CurrentHitsInAbility = 0;
        CurrentHitsInTurn = 0;
        ResetTempStats();
    }
}