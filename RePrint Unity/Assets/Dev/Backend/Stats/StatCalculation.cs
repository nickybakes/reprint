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
    public static AbilityResults GetPlayerAbilityResult(AbilitySelection selection, AbilitySequence abilitySequence, BattleManager battleManager)
    {
        // TODO: Check the conditions of behaviors and compile the list of effects

        List<AbilityBehavior> behaviors = new List<AbilityBehavior>(selection.Ability.GetAbilityBehaviors(selection.Overclock));

        List<bool> passingBehaviors = new List<bool>();

        foreach (AbilityBehavior behavior in behaviors)
        {
            if (DoGameConditionsPass(behavior.Conditions, battleManager))
            {
                passingBehaviors.Add(true);
            }
        }

        List<AbilityEffect> effects = selection.Ability.GetAbilityEffects(selection.Ability.GetAbilityBehaviors(selection.Overclock), passingBehaviors);

        return GetAbilityResults(battleManager.Player, selection.Target, effects, battleManager);
    }

    public static AbilityResults GetEnemyAbilityResult(EnemyAbility ability, EnemyCharacter activator, BattleManager battleManager)
    {
        // TODO: Check the conditions of behaviors and compile the list of effects

        List<AbilityBehavior> behaviors = new List<AbilityBehavior>(ability.GetAbilityBehaviors());

        List<bool> passingBehaviors = new List<bool>();

        foreach (AbilityBehavior behavior in behaviors)
        {
            if (DoGameConditionsPass(behavior.Conditions, battleManager))
            {
                passingBehaviors.Add(true);
            }
        }

        List<AbilityEffect> effects = ability.GetAbilityEffects(ability.GetAbilityBehaviors(), passingBehaviors);

        return GetAbilityResults(activator, null, effects, battleManager);
    }

    public static AbilityResults GetAbilityResults(Character activator, Character target, List<AbilityEffect> effects, BattleManager manager)
    {
        AbilityAmounts damageAmounts = GetPotentialPhysicalDamage(activator, target, effects, manager);
        AbilityAmounts dodgeAmounts = GetPotentialDodgeGain(activator, target, effects, manager);
        AbilityAmounts chainGainAmounts = GetPotentialChainGain(activator, target, effects, manager);
        AbilityAmounts chainSpentAmounts = GetPotentialChainSpent(activator, target, effects, manager);
        chainSpentAmounts.NegateAmounts();

        damageAmounts.ApplyAmountsToCharacters(StatType.PhysicalDamage);

        dodgeAmounts.ApplyAmountsToCharacters(StatType.Dodge);
        dodgeAmounts.ApplyPrioritiesToCharacter(activator, StatType.Dodge);

        chainGainAmounts.ApplyAmountsToCharacters(StatType.Chain);
        chainSpentAmounts.ApplyAmountsToCharacters(StatType.Chain);

        return new AbilityResults(manager.Player, manager.EnemyTeam);
    }

    public static AbilityAmounts GetPotentialEffectAmount(Character activator, Character target, List<AbilityEffect> effects, BattleManager manager)
    {
        AbilityAmounts amounts = new AbilityAmounts(manager.Player, manager.EnemyTeam);

        foreach (AbilityEffect effect in effects)
        {
            int amount = effect.GetAmount(activator.GameValues);

            foreach (AbilityEffectApplication application in effect.ApplicationModes)
            {
                switch (application.Mode)
                {
                    case AbilityEffectApplicationMode.Self:
                        amounts.AddAmountToCharacter(activator, amount, target, amount);
                        break;

                    case AbilityEffectApplicationMode.TargetedCharacter:
                        amounts.AddAmountToCharacter(target, amount);
                        break;

                    case AbilityEffectApplicationMode.Player:
                        amounts.AddAmountToCharacter(manager.Player, amount);
                        break;

                    case AbilityEffectApplicationMode.NonTargetedEnemies:
                        List<Character> possibleEnemies = new List<Character>();
                        foreach (Character member in manager.EnemyTeam.Members)
                        {
                            if (member != target && member.IsAlive)
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
                        foreach (Character member in manager.EnemyTeam.Members)
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

    public static AbilityAmounts GetPotentialPhysicalDamage(Character activator, Character target, List<AbilityEffect> effects, BattleManager manager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.DoDamage));

        return GetPotentialEffectAmount(activator, target, filteredEffects, manager);
    }

    public static AbilityAmounts GetPotentialChainGain(Character activator, Character target, List<AbilityEffect> effects, BattleManager manager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainChain));

        return GetPotentialEffectAmount(activator, target, filteredEffects, manager);
    }

    public static AbilityAmounts GetPotentialChainSpent(Character activator, Character target, List<AbilityEffect> effects, BattleManager manager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.SpendChain));

        return GetPotentialEffectAmount(activator, target, filteredEffects, manager);
    }

    public static AbilityAmounts GetPotentialDodgeGain(Character activator, Character target, List<AbilityEffect> effects, BattleManager manager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainDodge));

        return GetPotentialEffectAmount(activator, target, filteredEffects, manager);
    }

    public static bool DoGameConditionsPass(List<GameCondition> conditions, BattleManager battleManager)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            // TODO: Add logic for checking conditions. If one fails, return false
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

        foreach (AbilityEffect effect in effects)
        {
            totalAmount += effect.GetAmount(activator.GameValues, getMinimum, !getMinimum); ;
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