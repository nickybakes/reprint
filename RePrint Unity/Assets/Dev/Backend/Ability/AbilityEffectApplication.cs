using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityEffectApplication
{
    [SerializeField] private AbilityEffectApplicationMode applicationMode;
    [SerializeField] private ValueInput numberOfNonTargetedEnemies;
    [SerializeField] private NonTargetedEnemyPriority nonTargetedEnemyPriority;
    [SerializeField] private bool addValueToTargetPriority;
    public AbilityEffectApplicationMode Mode { get => applicationMode; }
    public ValueInput NumberOfNonTargetedEnemies { get => numberOfNonTargetedEnemies; }
    public NonTargetedEnemyPriority NonTargetedEnemyPriority { get => nonTargetedEnemyPriority; }
    public bool AddValueToTargetPriority { get => addValueToTargetPriority; }
}


public enum AbilityEffectApplicationMode
{
    Self,
    TargetedCharacter,
    Player,
    NonTargetedEnemies,
    AllEnemies,
}

public enum NonTargetedEnemyPriority
{
    Random,
}