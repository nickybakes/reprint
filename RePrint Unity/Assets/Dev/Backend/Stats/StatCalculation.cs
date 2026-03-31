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
    public static AbilityResults GetPlayerAbilityResult(AbilitySelection selection, AbilitySequence abilitySequence, Character player, Team enemyTeam)
    {
        // TODO: Check the conditions of behaviors and compile the list of effects

        List<AbilityBehavior> behaviors = new List<AbilityBehavior>(selection.Ability.GetAbilityBehaviors(selection.Overclock));

        List<bool> passingBehaviors = new List<bool>();

        foreach (AbilityBehavior behavior in behaviors)
        {
            if (BehaviorConditionsPass(behavior))
            {
                passingBehaviors.Add(true);
            }
        }

        List<AbilityEffect> effects = selection.Ability.GetAbilityEffects(selection.Ability.GetAbilityBehaviors(selection.Overclock), passingBehaviors);

        int potentialDamage = GetPotentialPhysicalDamage(player, effects);
        // TODO: Alter the total amount based on the player's current mod chips

        if (selection.Ability.TargetAllEnemies(selection.Overclock))
        {
            enemyTeam.ApplyPhysicalDamageToTeam(potentialDamage);
        }
        else
        {
            selection.Target.ApplyPhysicalDamage(potentialDamage);
        }

        int potentialChainGain = GetPotentialChainGain(player, effects);
        // TODO: Alter the total amount based on the player's current mod chips

        player.ApplyChain(potentialChainGain);

        int potentialChainLoss = GetPotentialChainLoss(player, effects);
        // TODO: Alter the total amount based on the player's current mod chips

        player.ApplyChain(-potentialChainLoss);


        int potentialDodge = GetPotentialDodgeGain(player, effects);
        // TODO: Alter the total amount based on the player's current mod chips

        player.ApplyDodge(potentialDodge, selection.Target);


        return new AbilityResults(player, enemyTeam);
    }

    public static bool BehaviorConditionsPass(AbilityBehavior behavior)
    {
        for (int i = 0; i < behavior.Conditions.Count; i++)
        {
            // TODO: Add logic for checking conditions
        }
        return true;
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

    public static int GetPotentialEffectAmount(Character activator, List<AbilityEffect> effects)
    {
        int totalAmount = 0;

        foreach (AbilityEffect effect in effects)
        {
            int baseAmount = effect.ValueInput.GetValue();

            foreach (Arithmetic arithmetic in effect.ExtraArithmetics)
            {
                baseAmount = arithmetic.CalculateSolution(baseAmount, activator.GameValues.GetInGameValue(arithmetic.GameValueType));
            }

            totalAmount += baseAmount;
        }

        return totalAmount;
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
                if (FilterForEffectType(effects, AbilityEffectType.RemoveAllChain).Count > 0)
                {
                    totalAmount -= activator.Stats.Chain;
                }
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
            int amount = effect.ValueInput.GetMaxValue();

            if (getMinimum)
                amount = effect.ValueInput.GetMinValue();

            foreach (Arithmetic arithmetic in effect.ExtraArithmetics)
            {
                amount = arithmetic.CalculateSolution(amount, activator.GameValues.GetInGameValue(arithmetic.GameValueType));
            }

            totalAmount += amount;
        }

        return totalAmount;
    }

    public static int GetPotentialPhysicalDamage(Character activator, List<AbilityEffect> effects)
    {
        return GetPotentialEffectAmount(activator, FilterForEffectType(effects, AbilityEffectType.DoDamage));
    }

    public static int GetPotentialChainGain(Character activator, List<AbilityEffect> effects)
    {
        int total = GetPotentialEffectAmount(activator, FilterForEffectType(effects, AbilityEffectType.GainChain));
        List<AbilityEffect> removeAllChainEffects = FilterForEffectType(effects, AbilityEffectType.RemoveAllChain);
        if (removeAllChainEffects.Count > 0)
        {
            total = activator.Stats.Chain;
        }
        return total;
    }

    public static int GetPotentialChainLoss(Character activator, List<AbilityEffect> effects)
    {
        int total = 0;
        List<AbilityEffect> removeAllChainEffects = FilterForEffectType(effects, AbilityEffectType.RemoveAllChain);
        if (removeAllChainEffects.Count > 0)
        {
            total = activator.Stats.Chain;
        }

        return Math.Min(total, activator.Stats.Chain);
    }

    public static int GetPotentialDodgeGain(Character activator, List<AbilityEffect> effects)
    {
        return GetPotentialEffectAmount(activator, FilterForEffectType(effects, AbilityEffectType.GainDodge));
    }

}