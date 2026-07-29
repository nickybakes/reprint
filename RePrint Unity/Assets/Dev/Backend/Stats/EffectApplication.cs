using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectApplication
{
    [SerializeField] private EffectApplicationMode applicationMode;
    [SerializeField] private ValueInput numberOfEnemies;
    [SerializeField] private NonTargetedEnemyPriority nonTargetedEnemyPriority;
    [SerializeField] private bool addValueToTargetPriority;
    public EffectApplicationMode Mode { get => applicationMode; }
    public ValueInput NumberOfEnemies { get => numberOfEnemies; }
    public NonTargetedEnemyPriority NonTargetedEnemyPriority { get => nonTargetedEnemyPriority; }
    public bool AddValueToTargetPriority { get => addValueToTargetPriority; }

    [SerializeField] private bool canRepeatEnemies;
    public bool CanRepeatEnemies { get => canRepeatEnemies; }


}


public enum EffectApplicationMode
{
    Self,
    TargetedCharacter,
    Player,
    RandomEnemies,
    NonTargetedEnemies,
    AllEnemies,
}

public enum NonTargetedEnemyPriority
{
    Random,
}