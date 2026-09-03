using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    PhysicalDamage,
    Dodge,
    Chain,
    MaxAP,
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
            ability = selection.Ability,
        };

        foreach (AbilityBehavior behavior in behaviors)
        {
            passingBehaviors.Add(DoGameConditionsPass(behavior.Conditions, gameValues));
        }

        if (selection.Ability.Type == AbilityType.Starter)
        {
            battleManager.Player.CurrentCombo++;
        }

        List<AbilityEffect> effects = selection.Ability.GetAbilityEffects(selection.Ability.GetAbilityBehaviors(selection.Overclock), passingBehaviors);

        StatChangeBreakdown results = GetAbilityStatChangeBreakdown(gameValues, effects);

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
            StatChangeBreakdown statChangeBreakdown = new StatChangeBreakdown(null, modResults, gameValues.activator);
            gameValues.battleManager.Player.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);

            foreach (ModResult modResult in modResults)
            {
                foreach (AbilitySelection abilityRetrigger in modResult.retriggerAbilities)
                {
                    AbilitySelection newAbilitySelection = new AbilitySelection(abilityRetrigger, true, modResult.mod);
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

        List<AbilityEffect> effects = ability.GetAbilityEffects(ability.GetAbilityBehaviors(), passingBehaviors);

        return GetAbilityStatChangeBreakdown(gameValues, effects);
    }

    public static StatChangeBreakdown GetAbilityStatChangeBreakdown(GameValues gameValues, List<AbilityEffect> effects)
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
        CalculateAbilityEffects(gameValues, effects, abilityStatChanges);
        List<ModResult> modResults = new List<ModResult>();
        StatChangeBreakdown statChangeBreakdown = new StatChangeBreakdown(abilityStatChanges, modResults, gameValues.activator);

        gameValues.currentStatChangeBreakdown = statChangeBreakdown;
        for (int i = 0; i < abilityStatChanges.GetInstanceCount(); i++)
        {
            gameValues.gameEvent = GameEvent.OnThisCharacterUsesAbility;
            gameValues.onLastInstance = i == abilityStatChanges.GetInstanceCount() - 1;
            gameValues.activator.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
            int hitAmount = (int)abilityStatChanges.GetTotalAmount(gameValues.activator, StatChange.HitAmountIncrease, i);
            for (int j = 0; j < hitAmount; j++)
            {
                gameValues.gameEvent = GameEvent.OnThisCharacterHits;
                gameValues.activator.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
            }
        }

        // Calculate Mod Stat Changes for victims
        foreach (Character character in nonActivatorCharacters)
        {
            for (int i = 0; i < abilityStatChanges.GetInstanceCount(); i++)
            {
                gameValues.gameEvent = GameEvent.OnOtherCharacterUsesAbility;

                gameValues.onLastInstance = i == abilityStatChanges.GetInstanceCount() - 1;
                character.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
                int hitAmount = (int)abilityStatChanges.GetTotalAmount(gameValues.activator, StatChange.HitAmountIncrease, i);
                for (int j = 0; j < hitAmount; j++)
                {
                    gameValues.gameEvent = GameEvent.OnThisCharacterGetsHit;
                    gameValues.activator.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
                }
            }
        }

        statChangeBreakdown.ApplyStatChanges(gameValues.battleManager.Player, gameValues.battleManager.EnemyTeam);

        return statChangeBreakdown;
    }

    public static void CalculateAbilityEffects(GameValues gameValues, List<AbilityEffect> effects, StatChangeAmounts statChanges, Character currentAffectedCharacter = null, bool getMinimum = false, bool getMaximum = false)
    {
        foreach (AbilityEffect effect in effects)
        {
            int occurences = effect.GetOcurrences(gameValues, getMinimum, getMaximum);

            for (int i = 0; i < occurences; i++)
            {
                if (effect.NewInstancePerOccurrence)
                    statChanges.StartNewInstance();

                List<Character> affectedCharacters = GetAffectedCharacters(gameValues, effect.ApplicationModes, currentAffectedCharacter);

                foreach (Character character in affectedCharacters)
                {
                    switch (effect.Type)
                    {
                        case AbilityEffectType.DoDamage:
                            float damage = effect.GetAmount(gameValues, getMinimum, getMaximum);
                            if (gameValues.abilityType == AbilityType.Finisher)
                            {
                                statChanges.AddAmount(character, damage, StatChange.FinisherPhysicalDamageTaken);
                            }
                            else
                            {
                                statChanges.AddAmount(character, damage, StatChange.StarterPhysicalDamageTaken);
                            }
                            if (!effect.DontAutoCountHits)
                            {
                                statChanges.AddAmount(gameValues.activator, 1, StatChange.HitAmountIncrease);
                                gameValues.activator.CurrentHitsInAbility++;
                                gameValues.activator.CurrentHitsInTurn++;
                            }
                            if (!effect.DontAddCharacterToUniqueHitList)
                            {
                                gameValues.activator.AddUniqueHitCharacter(character);
                            }
                            break;
                        case AbilityEffectType.GainDodge:
                            float dodge = effect.GetAmount(gameValues, getMinimum, getMaximum);
                            if (gameValues.activator == gameValues.battleManager.Player && gameValues.target != null)
                            {
                                statChanges.AddAmount(gameValues.target, dodge, StatChange.TurnPriorityGained);
                            }
                            statChanges.AddAmount(character, dodge, StatChange.DodgeGained);
                            break;
                        case AbilityEffectType.GainChain:
                            float chainGain = effect.GetAmount(gameValues, getMinimum, getMaximum);
                            statChanges.AddAmount(character, chainGain, StatChange.ChainGained);
                            break;
                        case AbilityEffectType.SpendChain:
                            float chainSpent = effect.GetAmount(gameValues, getMinimum, getMaximum);
                            statChanges.AddAmount(character, chainSpent, StatChange.ChainSpent);
                            break;
                        case AbilityEffectType.CountHits:
                            int hitCountAmount = effect.GetHitAmount(gameValues, getMinimum, getMaximum);
                            statChanges.AddAmount(gameValues.activator, hitCountAmount, StatChange.HitAmountIncrease);
                            gameValues.activator.CurrentHitsInAbility += hitCountAmount;
                            gameValues.activator.CurrentHitsInTurn += hitCountAmount;
                            if (!effect.DontAddCharacterToUniqueHitList)
                            {
                                gameValues.activator.AddUniqueHitCharacter(character);
                            }
                            break;
                    }

                    if (effect.ExtraEffects != null && effect.ExtraEffects.Count > 0)
                    {
                        CalculateAbilityEffects(gameValues, effect.ExtraEffects, statChanges, character, getMinimum, getMaximum);
                    }
                }
            }
        }
    }

    public static void CalculateModEffects(GameValues gameValues, Mod mod, List<ModEffect> effects, ModResult modResult, Character modOwner)
    {
        foreach (ModEffect effect in effects)
        {
            int occurences = effect.GetOcurrences(gameValues);

            for (int i = 0; i < occurences; i++)
            {
                if (effect.Type == ModEffectType.RetriggerAbility)
                {
                    modResult.retriggerAbilities.Add(mod.internalAbilitySelectionStorage[effect.IntValue1]);
                    continue;
                }

                List<Character> affectedCharacters = new List<Character>();

                if (effect.Type == ModEffectType.StackDamageMultiplier || effect.Type == ModEffectType.StackCritChance)
                {
                    affectedCharacters.Add(modOwner);
                }
                else
                {
                    affectedCharacters = GetAffectedCharacters(gameValues, effect.ApplicationModes);
                }

                foreach (Character character in affectedCharacters)
                {
                    switch (effect.Type)
                    {
                        case ModEffectType.DoDamage:
                            float damage = effect.GetAmount(gameValues);
                            modResult.statChangeAmounts.AddAmount(character, damage, StatChange.KineticDamageTaken);
                            break;
                        case ModEffectType.StackDamageMultiplier:
                            float multiplier = effect.GetAmount(gameValues);
                            if (effect.StarterActions)
                            {
                                modResult.statChangeAmounts.AddAmount(character, multiplier, StatChange.StarterPhysicalDamageMultiplier);
                            }
                            if (effect.FinisherActions)
                            {
                                modResult.statChangeAmounts.AddAmount(character, multiplier, StatChange.FinisherPhysicalDamageMultiplier);
                            }
                            break;
                        case ModEffectType.GainChain:
                            float chainGain = effect.GetAmount(gameValues);
                            modResult.statChangeAmounts.AddAmount(character, chainGain, StatChange.ChainGained);
                            break;
                        case ModEffectType.GainDodge:
                            float dodgeGain = effect.GetAmount(gameValues);
                            modResult.statChangeAmounts.AddAmount(character, dodgeGain, StatChange.DodgeGained);
                            break;
                        case ModEffectType.GainMaxAP:
                            float maxAPGain = effect.GetAmount(gameValues);
                            modResult.statChangeAmounts.AddAmount(character, maxAPGain, StatChange.APMaxIncrease);
                            break;
                        case ModEffectType.StackCritChance:
                            float critChance = effect.GetAmount(gameValues);
                            modResult.statChangeAmounts.AddAmount(character, critChance, StatChange.CritChanceIncrease);
                            break;
                    }
                }
            }
        }
    }

    public static List<Character> GetAffectedCharacters(GameValues gameValues, List<EffectApplication> applicationModes, Character currentAffectedCharacter = null)
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

                case EffectApplicationMode.RandomEnemies:
                    List<Character> possibleRandomEnemies = new List<Character>();
                    foreach (Character member in gameValues.battleManager.EnemyTeam.Members)
                    {
                        if (member.IsAlive)
                        {
                            possibleRandomEnemies.Add(member);
                        }
                    }

                    int totalCharacters = (int)application.NumberOfEnemies.GetValue();
                    int numCharacters = 0;

                    while (numCharacters < totalCharacters && possibleRandomEnemies.Count > 0)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, possibleRandomEnemies.Count);
                        affectedCharacters.Add(possibleRandomEnemies[randomIndex]);
                        if (!application.CanRepeatEnemies)
                            possibleRandomEnemies.RemoveAt(randomIndex);
                        numCharacters++;
                    }

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

                    totalCharacters = (int)application.NumberOfEnemies.GetValue();
                    numCharacters = 0;

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
                    affectedCharacters.AddRange(GetAllEnemies(gameValues));
                    break;
                case EffectApplicationMode.CurrentAffectedCharacter:
                    if (currentAffectedCharacter != null)
                    {
                        affectedCharacters.Add(currentAffectedCharacter);
                    }
                    break;
            }
        }

        return affectedCharacters;
    }

    public static List<Character> GetAllEnemies(GameValues gameValues)
    {
        List<Character> affectedCharacters = new List<Character>();

        foreach (Character member in gameValues.battleManager.EnemyTeam.Members)
        {
            if (member.IsAlive)
            {
                affectedCharacters.Add(member);
            }
        }

        return affectedCharacters;
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

                    if (condition.GameEvent == GameEvent.OnThisCharacterUsesAbility || condition.GameEvent == GameEvent.OnOtherCharacterUsesAbility)
                    {
                        if (condition.OnlyOnOneInstance && gameValues.onLastInstance != true)
                            return false;
                    }
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
                            case TurnStat.TotalCombo:
                                amountTurn = turnResults.totalCombo;
                                break;
                            case TurnStat.TotalHits:
                                amountTurn = turnResults.totalHits;
                                break;
                            case TurnStat.UniqueEnemiesHit:
                                amountTurn = turnResults.uniqueCharactersHit.Count;
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

    public static StatChangeAmounts GetMinOrMaxStat(BattleManager battleManager, bool getMinimum, Character activator, Character target, List<AbilityEffect> effects)
    {
        StatChangeAmounts statChanges = new StatChangeAmounts(battleManager.Player, battleManager.EnemyTeam);
        GameValues gameValues = new GameValues()
        {
            battleManager = battleManager,
            activator = activator,
            target = target
        };

        CalculateAbilityEffects(gameValues, effects, statChanges, null, getMinimum, !getMinimum);

        return statChanges;
    }

    public static float GetMinOrMaxEffectAmount(bool getMinimum, Character activator, Character target, List<AbilityEffect> effects)
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