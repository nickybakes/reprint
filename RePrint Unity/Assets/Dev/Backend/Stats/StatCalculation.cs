using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    PhysicalDamage,
    Dodge,
    Chain,
}

public class StatCalculation
{
    public static StatChangeBreakdown GetPlayerAbilityStatChangeBreakdown(AbilitySelection selection, int abilitySeqIndex, AbilitySequence abilitySequence, BattleManager battleManager)
    {
        List<AbilityBehavior> behaviors = new List<AbilityBehavior>(selection.Ability.GetAbilityBehaviors(selection.Overclock));

        List<bool> passingBehaviors = new List<bool>();

        GameValues gameValues = new GameValues
        {
            battleManager = battleManager,
            activator = battleManager.Player,
            target = selection.Target,
        };

        foreach (AbilityBehavior behavior in behaviors)
        {
            passingBehaviors.Add(DoGameConditionsPass(behavior.Conditions, gameValues));
        }

        List<AbilityEffect> effects = selection.Ability.GetAbilityEffects(selection.Ability.GetAbilityBehaviors(selection.Overclock), passingBehaviors);

        StatChangeBreakdown results = GetAbilityStatChangeBreakdown(battleManager.Player, selection.Target, effects, battleManager);

        return results;
    }

    public static StatChangeBreakdown GetEnemyAbilityStatChangeBreakdown(EnemyAbility ability, EnemyCharacter activator, BattleManager battleManager)
    {
        List<AbilityBehavior> behaviors = new List<AbilityBehavior>(ability.GetAbilityBehaviors());

        List<bool> passingBehaviors = new List<bool>();

        GameValues gameValues = new GameValues
        {
            battleManager = battleManager,
            activator = activator,
        };

        foreach (AbilityBehavior behavior in behaviors)
        {
            passingBehaviors.Add(DoGameConditionsPass(behavior.Conditions, gameValues));
        }

        List<AbilityEffect> effects = ability.GetAbilityEffects(ability.GetAbilityBehaviors(), passingBehaviors);

        return GetAbilityStatChangeBreakdown(activator, null, effects, battleManager);
    }

    public static StatChangeBreakdown GetAbilityStatChangeBreakdown(Character activator, Character target, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<Character> nonActivatorCharacters = new List<Character>();

        if (activator != battleManager.Player)
        {
            nonActivatorCharacters.Add(battleManager.Player);
        }

        foreach (Character enemy in battleManager.EnemyTeam.Members)
        {
            if (enemy != activator)
            {
                nonActivatorCharacters.Add(enemy);
            }
        }

        GameValues gameValues = new GameValues
        {
            battleManager = battleManager,
            activator = activator,
            target = target,
            gameEvent = GameEvent.OnCharacterUsesAbility,
        };

        StatChangeAmounts abilityStatChanges = new StatChangeAmounts(battleManager.Player, battleManager.EnemyTeam, StatChangeSource.FromAbility);
        CalculatePotentialPhysicalDamage(abilityStatChanges, gameValues, effects, battleManager);
        CalculatePotentialDodgeGain(abilityStatChanges, gameValues, effects, battleManager);
        CalculatePotentialChainGain(abilityStatChanges, gameValues, effects, battleManager);
        CalculatePotentialChainSpent(abilityStatChanges, gameValues, effects, battleManager);

        List<StatChangeAmounts> modStatChanges = new List<StatChangeAmounts>();
        StatChangeBreakdown statChangeBreakdown = new StatChangeBreakdown(abilityStatChanges, modStatChanges);

        gameValues.currentStatChangeBreakdown = statChangeBreakdown;
        activator.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);

        // Calculate Mod Stat Changes for victims
        gameValues.gameEvent = GameEvent.OnOtherCharacterUsesAbility;
        foreach (Character character in nonActivatorCharacters)
        {
            character.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
        }

        statChangeBreakdown.ApplyStatChanges(battleManager.Player, battleManager.EnemyTeam);

        return statChangeBreakdown;
    }

    public static AmountsPerCharacter GetPotentialEffectAmount(GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        AmountsPerCharacter amounts = new AmountsPerCharacter(battleManager.Player, battleManager.EnemyTeam);

        foreach (AbilityEffect effect in effects)
        {
            int amount = effect.GetAmount(gameValues);

            foreach (AbilityEffectApplication application in effect.ApplicationModes)
            {
                switch (application.Mode)
                {
                    case AbilityEffectApplicationMode.Self:
                        amounts.AddAmountToCharacter(gameValues.activator, amount, gameValues.target, amount);
                        break;

                    case AbilityEffectApplicationMode.TargetedCharacter:
                        amounts.AddAmountToCharacter(gameValues.target, amount);
                        break;

                    case AbilityEffectApplicationMode.Player:
                        amounts.AddAmountToCharacter(battleManager.Player, amount);
                        break;

                    case AbilityEffectApplicationMode.NonTargetedEnemies:
                        List<Character> possibleEnemies = new List<Character>();
                        foreach (Character member in battleManager.EnemyTeam.Members)
                        {
                            if (member != gameValues.target && member.IsAlive)
                            {
                                possibleEnemies.Add(member);
                            }
                        }

                        int totalCharacters = application.NumberOfNonTargetedEnemies.GetValue();
                        int numCharacters = 0;

                        switch (application.NonTargetedEnemyPriority)
                        {
                            case NonTargetedEnemyPriority.Random:
                                while (numCharacters < totalCharacters && possibleEnemies.Count > 0)
                                {
                                    int randomIndex = UnityEngine.Random.Range(0, possibleEnemies.Count);
                                    amounts.AddAmountToCharacter(possibleEnemies[randomIndex], amount);
                                    possibleEnemies.RemoveAt(randomIndex);
                                    numCharacters++;
                                }
                                break;
                        }

                        break;

                    case AbilityEffectApplicationMode.AllEnemies:
                        foreach (Character member in battleManager.EnemyTeam.Members)
                        {
                            if (member.IsAlive)
                            {
                                amounts.AddAmountToCharacter(member, amount);
                            }
                        }
                        break;
                }
            }


        }

        return amounts;
    }

    public static void CalculatePotentialPhysicalDamage(StatChangeAmounts statChanges, GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.DoDamage));

        AmountsPerCharacter amounts = GetPotentialEffectAmount(gameValues, filteredEffects, battleManager);

        statChanges.AddAmounts(amounts.Amounts, StatChange.PhysicalDamageTaken);
    }

    public static void CalculatePotentialChainGain(StatChangeAmounts statChanges, GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainChain));

        AmountsPerCharacter amounts = GetPotentialEffectAmount(gameValues, filteredEffects, battleManager);

        statChanges.AddAmounts(amounts.Amounts, StatChange.ChainGained);
    }

    public static void CalculatePotentialChainSpent(StatChangeAmounts statChanges, GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.SpendChain));

        AmountsPerCharacter amounts = GetPotentialEffectAmount(gameValues, filteredEffects, battleManager);

        statChanges.AddAmounts(amounts.Amounts, StatChange.ChainSpent);
    }

    public static void CalculatePotentialDodgeGain(StatChangeAmounts statChanges, GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainDodge));

        AmountsPerCharacter amounts = GetPotentialEffectAmount(gameValues, filteredEffects, battleManager);

        statChanges.AddAmounts(amounts.Amounts, StatChange.DodgeGained);
        statChanges.AddAmounts(amounts.Priorities, StatChange.TurnPriorityGained);
    }

    public static bool DoGameConditionsPass(List<GameCondition> conditions, GameValues gameValues)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            GameCondition condition = conditions[i];
            switch (condition.Type)
            {
                case GameConditionType.OnGameEvent:
                    break;
                case GameConditionType.CharacterStat:
                    break;
            }
        }

        return true;
    }

    public static int GetMinOrMaxStat(bool getMinimum, Character activator, List<AbilityEffect> effects, StatType type)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        switch (type)
        {
            case StatType.PhysicalDamage:
                filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.DoDamage));
                break;
            case StatType.Chain:
                int totalAmount = GetMinOrMaxEffectAmount(getMinimum, activator, FilterForEffectType(effects, AbilityEffectType.GainChain));
                totalAmount -= GetMinOrMaxEffectAmount(getMinimum, activator, FilterForEffectType(effects, AbilityEffectType.SpendChain));
                return totalAmount;
            case StatType.Dodge:
                filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainDodge));
                break;
        }

        return GetMinOrMaxEffectAmount(getMinimum, activator, filteredEffects);

    }

    public static int GetMinOrMaxEffectAmount(bool getMinimum, Character activator, List<AbilityEffect> effects)
    {
        int totalAmount = 0;

        GameValues gameValues = new GameValues()
        {
            activator = activator
        };

        foreach (AbilityEffect effect in effects)
        {
            totalAmount += effect.GetAmount(gameValues, getMinimum, !getMinimum); ;
        }

        return totalAmount;
    }



    public static List<AbilityEffect> FilterForEffectType(List<AbilityEffect> effects, AbilityEffectType type)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();

        foreach (AbilityEffect effect in effects)
        {
            if (effect.Type == type)
            {
                filteredEffects.Add(effect);
            }
        }

        return filteredEffects;
    }

}