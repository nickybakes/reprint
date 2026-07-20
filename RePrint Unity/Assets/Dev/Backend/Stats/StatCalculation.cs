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
    public static AbilityResults GetPlayerAbilityResult(AbilitySelection selection, int abilitySeqIndex, AbilitySequence abilitySequence, BattleManager battleManager)
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

        AbilityResults results = GetAbilityResults(battleManager.Player, selection.Target, effects, battleManager);

        return results;
    }

    public static AbilityResults GetEnemyAbilityResult(EnemyAbility ability, EnemyCharacter activator, BattleManager battleManager)
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

        return GetAbilityResults(activator, null, effects, battleManager);
    }

    public static AbilityResults GetAbilityResults(Character activator, Character target, List<AbilityEffect> effects, BattleManager battleManager)
    {
        GameValues gameValues = new GameValues
        {
            battleManager = battleManager,
            activator = activator,
            target = target,
        };
        gameValues.physicalDamageAmounts = GetPotentialPhysicalDamage(gameValues, effects, battleManager);
        gameValues.dodgeAmounts = GetPotentialDodgeGain(gameValues, effects, battleManager);
        gameValues.chainGainAmounts = GetPotentialChainGain(gameValues, effects, battleManager);
        gameValues.chainSpentAmounts = GetPotentialChainSpent(gameValues, effects, battleManager);
        gameValues.chainSpentAmounts.NegateAmounts();

        // TODO: Use activator's mods to affect the damage, dodge, etc
        for (int i = 0; i < activator.Mods.Count; i++)
        {
            Mod mod = activator.Mods[i];
            List<bool> passingBehaviors = new List<bool>();

            foreach (ModBehavior behavior in mod.Behaviors)
            {
                if (DoGameConditionsPass(behavior.Conditions, gameValues))
                {
                    passingBehaviors.Add(true);
                }
            }
        }

        gameValues.physicalDamageAmounts.ApplyAmountsToCharacters(StatType.PhysicalDamage);
        gameValues.dodgeAmounts.ApplyAmountsToCharacters(StatType.Dodge);
        gameValues.dodgeAmounts.ApplyPrioritiesToCharacter(activator, StatType.Dodge);
        gameValues.chainGainAmounts.ApplyAmountsToCharacters(StatType.Chain);
        gameValues.chainSpentAmounts.ApplyAmountsToCharacters(StatType.Chain);

        return new AbilityResults(battleManager.Player, battleManager.EnemyTeam);
    }

    public static AbilityAmounts GetPotentialEffectAmount(GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        AbilityAmounts amounts = new AbilityAmounts(battleManager.Player, battleManager.EnemyTeam);

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

    public static AbilityAmounts GetPotentialPhysicalDamage(GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.DoDamage));

        return GetPotentialEffectAmount(gameValues, filteredEffects, battleManager);
    }

    public static AbilityAmounts GetPotentialChainGain(GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainChain));

        return GetPotentialEffectAmount(gameValues, filteredEffects, battleManager);
    }

    public static AbilityAmounts GetPotentialChainSpent(GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.SpendChain));

        return GetPotentialEffectAmount(gameValues, filteredEffects, battleManager);
    }

    public static AbilityAmounts GetPotentialDodgeGain(GameValues gameValues, List<AbilityEffect> effects, BattleManager battleManager)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainDodge));

        return GetPotentialEffectAmount(gameValues, filteredEffects, battleManager);
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