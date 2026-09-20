using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{

    public CharacterProfile Profile { get; protected set; }
    public CharacterStats Stats { get; protected set; }
    public CharacterStats TurnStatsStorage { get; protected set; }

    public float CritDamageMultiplier { get; protected set; }
    public float BaseCritChance { get; protected set; }


    public int CurrentHitsInAbility { get; set; }
    public int CurrentHitsInTurn { get; set; }

    public List<Character> UniqueCharactersHitThisTurn { get; set; }

    public int CurrentCombo { get; set; }
    public int TotalCombo { get; set; }

    public List<Mod> Mods { get; protected set; }

    public string Name { get; protected set; }
    public int BaseMaxAP { get; protected set; }

    public BattleManager battleManager;

    public bool IsAlive
    {
        get
        {
            return Stats.Health > 0;
        }
    }

    public Character()
    {
        Stats = new CharacterStats(this);
        TurnStatsStorage = new CharacterStats(this);
    }

    public void SetTurnStats()
    {
        TurnStatsStorage.CopyFrom(Stats);
    }

    public void RestoreTurnStats()
    {
        Stats.CopyFrom(TurnStatsStorage);
    }

    public abstract void ResetForTurn();

    public void ApplyPhysicalDamage(int damage, float damageMultiplier, float critMultiplier, bool isCrit)
    {
        int totalDamage = (int)(damage * damageMultiplier);

        if (isCrit)
        {
            totalDamage = (int)(damage * damageMultiplier * critMultiplier);
        }

        // Use dodge first
        int tempDamage = totalDamage;
        totalDamage = Math.Max(0, totalDamage - Stats.Dodge);
        Stats.Dodge = Math.Max(0, Stats.Dodge - tempDamage);

        if (totalDamage > 0)
        {
            Stats.Health -= totalDamage;
            Stats.Chain = 0;

            if (isCrit)
            {
                Stats.CriticalDamageTaken += totalDamage;
            }
            else
            {
                Stats.PhysicalDamageTaken += totalDamage;
            }
        }
    }

    public void ApplyChain(int chain)
    {
        Stats.Chain = Math.Max(0, Stats.Chain + chain);
    }

    public void ApplyDodge(int dodge)
    {
        Stats.Dodge += dodge;
    }

    public void ResetTempStats()
    {
        Stats.TempChain = 0;
    }

    public void CalculateStatChangesFromMods(GameValues gameValues, StatChangeBreakdown statChangeBreakdown)
    {
        List<Mod> sortedMods = GetSortedMods();

        for (int i = 0; i < sortedMods.Count; i++)
        {
            Mod mod = sortedMods[i];
            ModResult modResult = new ModResult(mod, gameValues.battleManager.Player, gameValues.battleManager.EnemyTeam);
            modResult.statChangeAmounts.StartNewInstance();
            CalculateStatChangesFromMod(gameValues, modResult);
            statChangeBreakdown.modResults.Add(modResult);
        }
    }

    public void CalculateStatChangesFromModsFromAbilities(GameValues gameValues, StatChangeBreakdown statChangeBreakdown, StatChangeAmounts abilityStatChanges, bool thisCharacterUsesbility)
    {
        List<Mod> sortedMods = GetSortedMods();

        for (int i = 0; i < sortedMods.Count; i++)
        {
            Mod mod = sortedMods[i];

            ModResult modResult = new ModResult(mod, gameValues.battleManager.Player, gameValues.battleManager.EnemyTeam);

            for (int j = 0; j < abilityStatChanges.GetInstanceCount(); j++)
            {
                modResult.statChangeAmounts.StartNewInstance();
                gameValues.gameEvent = thisCharacterUsesbility ? GameEvent.OnThisCharacterUsesAbility : GameEvent.OnOtherCharacterUsesAbility;
                gameValues.currentMod = mod;
                gameValues.currentInstance = j;
                gameValues.onLastInstance = j == abilityStatChanges.GetInstanceCount() - 1;

                CalculateStatChangesFromMod(gameValues, modResult);

                int hitAmount = (int)abilityStatChanges.GetTotalAmount(gameValues.activator, StatChange.HitAmountIncrease, i);
                for (int k = 0; k < hitAmount; k++)
                {
                    gameValues.gameEvent = thisCharacterUsesbility ? GameEvent.OnThisCharacterHits : GameEvent.OnThisCharacterGetsHit;
                    gameValues.activator.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
                }
            }

            statChangeBreakdown.modResults.Add(modResult);
        }
    }

    private void CalculateStatChangesFromMod(GameValues gameValues, ModResult modResult)
    {
        int numPassingBehaviors = 0;

        List<bool> passingBehaviors = new List<bool>();

        foreach (ModBehavior behavior in modResult.mod.Behaviors)
        {
            gameValues.currentMod = modResult.mod;
            bool pass = StatCalculation.DoGameConditionsPass(behavior.Conditions, gameValues);
            passingBehaviors.Add(pass);
            if (pass)
                numPassingBehaviors++;
        }

        if (numPassingBehaviors == 0)
        {
            return;
        }

        List<ModEffect> effects = modResult.mod.GetModEffects(passingBehaviors);
        StatCalculation.CalculateModEffects(gameValues, modResult.mod, effects, modResult, this);
    }

    public List<Mod> GetSortedMods()
    {
        List<Mod> sortedMods = new List<Mod>();
        List<Mod> remainingMods = new List<Mod>(Mods);

        while (remainingMods.Count > 0)
        {
            Mod lowestSortMod = remainingMods[0];
            int lowestSort = int.MaxValue;
            foreach (Mod mod in remainingMods)
            {
                if (mod.SortOrder < lowestSort)
                {
                    lowestSort = mod.SortOrder;
                    lowestSortMod = mod;
                }
            }

            sortedMods.Add(lowestSortMod);
            remainingMods.Remove(lowestSortMod);
        }

        return sortedMods;
    }

    public void AddUniqueHitCharacter(Character character)
    {
        if (character != this && !UniqueCharactersHitThisTurn.Contains(character))
        {
            UniqueCharactersHitThisTurn.Add(character);
        }
    }
}