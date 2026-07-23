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

        StatChangeBreakdown results = GetAbilityStatChangeBreakdown(battleManager.Player, selection.Target, effects, selection.Ability.Type, battleManager);

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

        return GetAbilityStatChangeBreakdown(activator, null, effects, AbilityType.Starter, battleManager);
    }

    public static StatChangeBreakdown GetAbilityStatChangeBreakdown(Character activator, Character target, List<AbilityEffect> effects, AbilityType abilityType, BattleManager battleManager)
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

        StatChangeAmounts abilityStatChanges = new StatChangeAmounts(battleManager.Player, battleManager.EnemyTeam);
        CalculatePotentialPhysicalDamage(abilityStatChanges, gameValues, effects, abilityType);
        CalculatePotentialDodgeGain(abilityStatChanges, gameValues, effects);
        CalculatePotentialChainGain(abilityStatChanges, gameValues, effects);
        CalculatePotentialChainSpent(abilityStatChanges, gameValues, effects);

        List<ModResult> modResults = new List<ModResult>();
        StatChangeBreakdown statChangeBreakdown = new StatChangeBreakdown(abilityStatChanges, modResults);

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

    public static AmountsPerCharacter GetPotentialEffectAmount(GameValues gameValues, List<Effect> effects)
    {
        AmountsPerCharacter amounts = new AmountsPerCharacter(gameValues.battleManager.Player, gameValues.battleManager.EnemyTeam);

        foreach (Effect effect in effects)
        {
            int amount = effect.GetAmount(gameValues);

            List<Character> affectedCharacters = GetAffectedCharacters(gameValues, effect.ApplicationModes);

            foreach (Character character in affectedCharacters)
            {
                if (character == gameValues.activator)
                {
                    amounts.AddAmountToCharacter(character, amount, gameValues.target, amount);
                }
                else
                {
                    amounts.AddAmountToCharacter(character, amount);
                }
            }
        }

        return amounts;
    }

    public static AmountsPerCharacter GetPotentialEffectAmount(GameValues gameValues, Effect effect)
    {
        return GetPotentialEffectAmount(gameValues, new List<Effect>(new Effect[] { effect }));
    }

    public static List<Character> GetAffectedCharacters(GameValues gameValues, List<EffectApplication> applicationModes)
    {
        List<Character> affectedCharacters = new List<Character>();

        foreach (EffectApplication application in applicationModes)
        {
            switch (application.Mode)
            {
                case EffectApplicationMode.Self:
                    affectedCharacters.Add(gameValues.activator);
                    break;

                case EffectApplicationMode.TargetedCharacter:
                    affectedCharacters.Add(gameValues.target);
                    break;

                case EffectApplicationMode.Player:
                    affectedCharacters.Add(gameValues.battleManager.Player);
                    break;

                case EffectApplicationMode.NonTargetedEnemies:
                    List<Character> possibleEnemies = new List<Character>();
                    foreach (Character member in gameValues.battleManager.EnemyTeam.Members)
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
                                affectedCharacters.Add(possibleEnemies[randomIndex]);
                                possibleEnemies.RemoveAt(randomIndex);
                                numCharacters++;
                            }
                            break;
                    }

                    break;

                case EffectApplicationMode.AllEnemies:
                    foreach (Character member in gameValues.battleManager.EnemyTeam.Members)
                    {
                        if (member.IsAlive)
                        {
                            affectedCharacters.Add(member);
                        }
                    }
                    break;
            }
        }

        return affectedCharacters;
    }

    public static void CalculatePotentialPhysicalDamage(StatChangeAmounts statChanges, GameValues gameValues, List<AbilityEffect> effects, AbilityType abilityType)
    {
        List<Effect> filteredEffects = new List<Effect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.DoDamage));

        AmountsPerCharacter amounts = GetPotentialEffectAmount(gameValues, filteredEffects);

        if (abilityType == AbilityType.Finisher)
        {
            statChanges.AddAmounts(amounts.Amounts, StatChange.FinisherPhysicalDamageTaken);
        }
        else
        {
            statChanges.AddAmounts(amounts.Amounts, StatChange.StarterPhysicalDamageTaken);
        }

    }

    public static void CalculatePotentialChainGain(StatChangeAmounts statChanges, GameValues gameValues, List<AbilityEffect> effects)
    {
        List<Effect> filteredEffects = new List<Effect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainChain));

        AmountsPerCharacter amounts = GetPotentialEffectAmount(gameValues, filteredEffects);

        statChanges.AddAmounts(amounts.Amounts, StatChange.ChainGained);
    }

    public static void CalculatePotentialChainSpent(StatChangeAmounts statChanges, GameValues gameValues, List<AbilityEffect> effects)
    {
        List<Effect> filteredEffects = new List<Effect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.SpendChain));

        AmountsPerCharacter amounts = GetPotentialEffectAmount(gameValues, filteredEffects);

        statChanges.AddAmounts(amounts.Amounts, StatChange.ChainSpent);
    }

    public static void CalculatePotentialDodgeGain(StatChangeAmounts statChanges, GameValues gameValues, List<AbilityEffect> effects)
    {
        List<Effect> filteredEffects = new List<Effect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainDodge));

        AmountsPerCharacter amounts = GetPotentialEffectAmount(gameValues, filteredEffects);

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
                    if (condition.GameEvent != gameValues.gameEvent)
                        return false;
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