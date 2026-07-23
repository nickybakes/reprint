using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectApplication
{
    [SerializeField] private EffectApplicationMode applicationMode;
    [SerializeField] private ValueInput numberOfNonTargetedEnemies;
    [SerializeField] private NonTargetedEnemyPriority nonTargetedEnemyPriority;
    [SerializeField] private bool addValueToTargetPriority;
    public EffectApplicationMode Mode { get => applicationMode; }
    public ValueInput NumberOfNonTargetedEnemies { get => numberOfNonTargetedEnemies; }
    public NonTargetedEnemyPriority NonTargetedEnemyPriority { get => nonTargetedEnemyPriority; }
    public bool AddValueToTargetPriority { get => addValueToTargetPriority; }
}


public enum EffectApplicationMode
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