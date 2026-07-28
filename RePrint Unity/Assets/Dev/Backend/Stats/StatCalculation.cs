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
            currentAbilitySelection = selection,
            abilityType = selection.Ability.Type,
        };

        foreach (AbilityBehavior behavior in behaviors)
        {
            passingBehaviors.Add(DoGameConditionsPass(behavior.Conditions, gameValues));
        }

        if (selection.Ability.Type == AbilityType.Starter)
        {
            battleManager.Player.CurrentCombo++;
        }

        List<AbilityHit> hits = selection.Ability.GetAbilityHits(selection.Ability.GetAbilityBehaviors(selection.Overclock), passingBehaviors);

        StatChangeBreakdown results = GetAbilityStatChangeBreakdown(gameValues, hits);

        return results;
    }

    public static void CheckAbilitySequence(List<AbilitySelection> sequence, AbilitySequence abilitySequence, BattleManager battleManager)
    {
        GameValues gameValues = new GameValues
        {
            battleManager = battleManager,
            activator = battleManager.Player,
            gameEvent = GameEvent.OnCheckAbilitySequence,
            abilitySequence = abilitySequence
        };

        for (int i = 0; i < sequence.Count; i++)
        {
            AbilitySelection abilitySelection = sequence[i];

            if (abilitySelection.Ability.Type == AbilityType.Starter)
            {
                battleManager.Player.CurrentCombo++;
            }

            gameValues.target = abilitySelection.Target;
            gameValues.currentAbilitySelection = abilitySelection;
            gameValues.abilityType = abilitySelection.Ability.Type;

            List<ModResult> modResults = new List<ModResult>();
            StatChangeBreakdown statChangeBreakdown = new StatChangeBreakdown(null, modResults);
            gameValues.battleManager.Player.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);

            foreach (ModResult modResult in modResults)
            {
                foreach (AbilitySelection abilityRetrigger in modResult.retriggerAbilities)
                {
                    AbilitySelection newAbilitySelection = new AbilitySelection(abilityRetrigger, true);
                    int index = sequence.IndexOf(abilityRetrigger);
                    sequence.Insert(index + 1, newAbilitySelection);
                }
            }
        }
    }

    public static int GetTotalCombo(List<AbilitySelection> sequence)
    {
        int amount = 0;

        foreach (AbilitySelection selection in sequence)
        {
            if (selection.Ability.Type == AbilityType.Starter)
            {
                amount++;
            }
        }

        return amount;
    }

    public static StatChangeBreakdown GetEnemyAbilityStatChangeBreakdown(EnemyAbility ability, EnemyCharacter activator, BattleManager battleManager)
    {
        List<AbilityBehavior> behaviors = new List<AbilityBehavior>(ability.GetAbilityBehaviors());

        List<bool> passingBehaviors = new List<bool>();

        GameValues gameValues = new GameValues
        {
            battleManager = battleManager,
            activator = activator,
            abilityType = AbilityType.Starter
        };

        foreach (AbilityBehavior behavior in behaviors)
        {
            passingBehaviors.Add(DoGameConditionsPass(behavior.Conditions, gameValues));
        }

        List<AbilityHit> hits = ability.GetAbilityHits(ability.GetAbilityBehaviors(), passingBehaviors);

        return GetAbilityStatChangeBreakdown(gameValues, hits);
    }

    public static StatChangeBreakdown GetAbilityStatChangeBreakdown(GameValues gameValues, List<AbilityHit> hits)
    {
        List<Character> nonActivatorCharacters = new List<Character>();

        if (gameValues.activator != gameValues.battleManager.Player)
        {
            nonActivatorCharacters.Add(gameValues.battleManager.Player);
        }

        foreach (Character enemy in gameValues.battleManager.EnemyTeam.Members)
        {
            if (enemy != gameValues.activator)
            {
                nonActivatorCharacters.Add(enemy);
            }
        }

        gameValues.gameEvent = GameEvent.OnThisCharacterUsesAbility;

        StatChangeAmounts abilityStatChanges = new StatChangeAmounts(gameValues.battleManager.Player, gameValues.battleManager.EnemyTeam);

        List<ModResult> modResults = new List<ModResult>();
        StatChangeBreakdown statChangeBreakdown = new StatChangeBreakdown(abilityStatChanges, modResults);

        gameValues.currentStatChangeBreakdown = statChangeBreakdown;
        gameValues.activator.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);

        int hitTotal = 0;

        gameValues.gameEvent = GameEvent.OnThisCharacterHits;

        for (int i = 0; i < hits.Count; i++)
        {
            AbilityHit hit = hits[i];
            int hitAmount = hit.GetAmount(gameValues);
            for (int j = 0; j < hitAmount; j++)
            {
                hitTotal += hitAmount;
                CalculatePotentialPhysicalDamage(abilityStatChanges, gameValues, hit.Effects, gameValues.abilityType);
                CalculatePotentialDodgeGain(abilityStatChanges, gameValues, hit.Effects);
                CalculatePotentialChainGain(abilityStatChanges, gameValues, hit.Effects);
                CalculatePotentialChainSpent(abilityStatChanges, gameValues, hit.Effects);
                gameValues.activator.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
            }
        }


        // Calculate Mod Stat Changes for victims
        gameValues.gameEvent = GameEvent.OnOtherCharacterUsesAbility;
        foreach (Character character in nonActivatorCharacters)
        {
            character.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
        }

        statChangeBreakdown.ApplyStatChanges(gameValues.battleManager.Player, gameValues.battleManager.EnemyTeam);

        return statChangeBreakdown;
    }

    public static AmountsPerCharacter GetPotentialEffectAmount(GameValues gameValues, List<Effect> effects)
    {
        AmountsPerCharacter amounts = new AmountsPerCharacter(gameValues.battleManager.Player, gameValues.battleManager.EnemyTeam);

        foreach (Effect effect in effects)
        {
            float amount = effect.GetAmount(gameValues);

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

    public static List<Character> GetAffectedCharacters(GameValues gameValues, EffectApplication effectApplication)
    {
        return GetAffectedCharacters(gameValues, new List<EffectApplication>(new EffectApplication[] { effectApplication }));
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

                    int totalCharacters = (int)application.NumberOfNonTargetedEnemies.GetValue();
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
                    List<Character> characters = GetAffectedCharacters(gameValues, condition.Characters);
                    foreach (Character character in characters)
                    {
                        if (!condition.CheckCharacterStatCondition(character, condition.ValueInput1.GetValue(), condition.Comparison1))
                        {
                            return false;
                        }
                    }
                    break;
                case GameConditionType.AbilityType:
                    if (condition.AbilityType != gameValues.abilityType)
                        return false;
                    break;
                case GameConditionType.ComboAmount:
                    int amount = 0;
                    switch (condition.ComboCountType)
                    {
                        case ComboCountType.Current:
                            amount = gameValues.battleManager.Player.CurrentCombo;
                            break;
                        case ComboCountType.Total:
                            amount = gameValues.battleManager.Player.TotalCombo;
                            break;
                    }
                    if (!condition.CheckComparison(amount, condition.ValueInput1.GetValue(), condition.Comparison1))
                    {
                        return false;
                    }
                    break;
                case GameConditionType.StoreAbilityInternally:
                    gameValues.currentMod.internalAbilitySelectionStorage[condition.IntValue1] = gameValues.currentAbilitySelection;
                    break;
                case GameConditionType.TurnHistory:
                    TurnResults turnResults = gameValues.battleManager.currentTurnResults;
                    switch (condition.TurnIndexType)
                    {
                        case TurnIndexType.PreviousTurn:
                            if (gameValues.battleManager.TurnIndex == 0)
                            {
                                return false;
                            }
                            turnResults = gameValues.battleManager.turnHistory[gameValues.battleManager.TurnIndex - 1];
                            break;
                    }
                    List<Character> charactersTurnHistory = GetAffectedCharacters(gameValues, condition.Characters);
                    foreach (Character character in charactersTurnHistory)
                    {
                        int amountTurn = 0;
                        switch (condition.TurnStat)
                        {
                            case TurnStat.TotalHealthLost:
                                amountTurn = turnResults.GetStatDifference(character, CharacterStat.Health);
                                amountTurn *= -1;
                                break;
                        }
                        if (!condition.CheckComparison(amountTurn, condition.ValueInput1.GetValue(), condition.Comparison1))
                        {
                            return false;
                        }
                    }

                    break;
                case GameConditionType.TurnOrWaveIndex:
                    int indexToCompare = gameValues.battleManager.TurnIndex;
                    switch (condition.TurnCountType)
                    {
                        case TurnCountType.TurnIndexInWave:
                            indexToCompare = gameValues.battleManager.TurnIndexInWave;
                            break;
                        case TurnCountType.WaveIndex:
                            indexToCompare = gameValues.battleManager.WaveIndex;
                            break;
                    }

                    switch (condition.IndexType)
                    {
                        case IndexType.First:
                            if (indexToCompare != 0)
                                return false;
                            break;
                        case IndexType.Middle:
                            if (indexToCompare == 0)
                                return false;
                            break;
                        case IndexType.Specific:
                            if (indexToCompare != (int)condition.ValueInput1.GetValue())
                                return false;
                            break;
                    }
                    break;
            }
        }

        return true;
    }

    public static float GetMinOrMaxStat(bool getMinimum, Character activator, List<AbilityEffect> effects, StatType type)
    {
        List<AbilityEffect> filteredEffects = new List<AbilityEffect>();
        switch (type)
        {
            case StatType.PhysicalDamage:
                filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.DoDamage));
                break;
            case StatType.Chain:
                float totalAmount = GetMinOrMaxEffectAmount(getMinimum, activator, FilterForEffectType(effects, AbilityEffectType.GainChain));
                totalAmount -= GetMinOrMaxEffectAmount(getMinimum, activator, FilterForEffectType(effects, AbilityEffectType.SpendChain));
                return totalAmount;
            case StatType.Dodge:
                filteredEffects.AddRange(FilterForEffectType(effects, AbilityEffectType.GainDodge));
                break;
        }

        return GetMinOrMaxEffectAmount(getMinimum, activator, filteredEffects);

    }

    public static float GetMinOrMaxEffectAmount(bool getMinimum, Character activator, List<AbilityEffect> effects)
    {
        float totalAmount = 0;

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