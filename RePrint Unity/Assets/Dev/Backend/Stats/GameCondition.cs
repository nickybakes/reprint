using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameCondition
{
    [SerializeField] private GameConditionType type;
    public GameConditionType Type { get => type; }

    [SerializeField] private GameEvent gameEvent;
    public GameEvent GameEvent { get => gameEvent; }

    [SerializeField] private CharacterStat characterStat;
    public CharacterStat CharacterStat { get => characterStat; }

    [SerializeField] protected BetterEditorList<EffectApplication> characters;

    public List<EffectApplication> Characters { get => characters.List; }

    [SerializeField] private ValueComparisonType comparison1;
    public ValueComparisonType Comparison1 { get => comparison1; }

    [SerializeField] private ValueInput valueInput1;
    public ValueInput ValueInput1 { get => valueInput1; }

    [SerializeField] private IndexType indexType;
    public IndexType IndexType { get => indexType; }

    [SerializeField] private AbilityType abilityType;
    public AbilityType AbilityType { get => abilityType; }

    public bool CheckCharacterStatCondition(Character character, float threshold, ValueComparisonType comparisonType)
    {
        int amount = character.Stats.GetStat(characterStat);
        return CheckComparison(amount, threshold, comparisonType);
    }

    public bool CheckComparison(float a, float b, ValueComparisonType comparisonType)
    {
        switch (comparisonType)
        {
            case ValueComparisonType.IsGreaterThan:
                return a > b;
            case ValueComparisonType.IsGreaterThanOrEqualTo:
                return a >= b;
            case ValueComparisonType.IsEqualTo:
                return a == b;
            case ValueComparisonType.IsNotEqualTo:
                return a != b;
            case ValueComparisonType.IsLessThan:
                return a < b;
            case ValueComparisonType.IsLessThanOrEqualTo:
                return a <= b;
        }

        return false;
    }

}

public enum GameConditionType
{
    OnGameEvent,
    CharacterStat,
    AbilityType,
    AbilitySequenceIndex
}

public enum ValueComparisonType
{
    IsGreaterThan,
    IsGreaterThanOrEqualTo,
    IsEqualTo,
    IsNotEqualTo,
    IsLessThan,
    IsLessThanOrEqualTo,

}

public enum GameEvent
{
    StartBattle,
    StartWave,
    PlayerTurnStart,
    OnCharacterUsesAbility,
    OnOtherCharacterUsesAbility,
    OnRefreshAbilitySequenceStats
}

public enum IndexType
{
    First,
    Last,
    Middle,
    Specific
}
