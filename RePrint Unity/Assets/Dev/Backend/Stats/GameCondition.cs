using System;
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

    [SerializeField] private AbilityEffectApplicationMode character;
    public AbilityEffectApplicationMode Character { get => character; }

    [SerializeField] private ValueComparisonType comparison1;
    public ValueComparisonType Comparison1 { get => comparison1; }

    [SerializeField] private ValueInput valueInput1;
    public ValueInput ValueInput1 { get => valueInput1; }

    [SerializeField] private IndexType indexType;
    public IndexType IndexType { get => indexType; }

}

public enum GameConditionType
{
    OnGameEvent,
    CharacterStat
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
    ChangeAbilitySequence,
    OnCharacterUsesAbility,
    OnOtherCharacterUsesAbility,
}

public enum IndexType
{
    First,
    Last,
    Middle,
    Specific
}
