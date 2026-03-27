using System;
using System.Collections.Generic;
using UnityEngine;

public class StatCalculation
{
    public static AbilityResults CalculatePlayerAbilityResult(AbilitySelection selection, AbilitySequence abilitySequence, Character player, Team enemyTeam)
    {
        // TODO: Check the conditions of behaviors and compile the list of effects
        List<AbilityEffect> effects = new List<AbilityEffect>();
        effects.AddRange(selection.Ability.GetAbilityEffects(selection.Overclock));

        AbilityRules rules = selection.Ability.GetAbilityRules(selection.Overclock);

        int potentialDamage = CalculatePotentialDamage(player, effects);
        // TODO: Alter the total amount based on the player's current mod chips

        if (rules.TargetAllEnemies)
        {
            enemyTeam.ApplyPhysicalDamageToTeam(potentialDamage);
        }
        else
        {
            selection.Target.ApplyPhysicalDamage(potentialDamage);
        }

        int potentialChainGain = CalculatePotentialChainGain(player, effects);
        // TODO: Alter the total amount based on the player's current mod chips

        player.ApplyChain(potentialChainGain);

        int potentialChainLoss = CalculatePotentialChainLoss(player, effects);
        // TODO: Alter the total amount based on the player's current mod chips

        player.ApplyChain(-potentialChainLoss);


        int potentialDodge = CalculatePotentialDodgeGain(player, effects);
        // TODO: Alter the total amount based on the player's current mod chips

        player.ApplyDodge(potentialDodge, selection.Target);


        return new AbilityResults(player, enemyTeam);
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

    public static int CalculatePotentialEffectAmount(Character activator, List<AbilityEffect> effects)
    {
        int totalAmount = 0;

        foreach (AbilityEffect effect in effects)
        {
            int baseAmount = effect.ValueInput.GetValue();

            foreach (Arithmetic modifier in effect.ExtraArithmetics)
            {
                baseAmount = modifier.CalculateSolution(baseAmount, activator.IncomingValues.GetIncomingValue(modifier.InGameValueType));
            }

            totalAmount += baseAmount;
        }

        return totalAmount;
    }

    public static int CalculatePotentialDamage(Character activator, List<AbilityEffect> effects)
    {
        return CalculatePotentialEffectAmount(activator, FilterForEffectType(effects, AbilityEffectType.DoDamage));
    }

    public static int CalculatePotentialChainGain(Character activator, List<AbilityEffect> effects)
    {
        return CalculatePotentialEffectAmount(activator, FilterForEffectType(effects, AbilityEffectType.GainChain));
    }

    public static int CalculatePotentialChainLoss(Character activator, List<AbilityEffect> effects)
    {
        int total = 0;
        List<AbilityEffect> removeAllChainEffects = FilterForEffectType(effects, AbilityEffectType.RemoveAllChain);
        if (removeAllChainEffects.Count > 0)
        {
            total = activator.Stats.Chain;
        }

        return Math.Min(total, activator.Stats.Chain);
    }

    public static int CalculatePotentialDodgeGain(Character activator, List<AbilityEffect> effects)
    {
        return CalculatePotentialEffectAmount(activator, FilterForEffectType(effects, AbilityEffectType.GainDodge));
    }

}