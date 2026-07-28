using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityRules
{
    [SerializeField] private int apCost;
    [SerializeField] private bool targetAllEnemies;
    [SerializeField] private bool canTargetPlayer;
    [SerializeField] private bool canTargetEnemies;

    public int APCost { get => apCost; }

    public bool CanTargetPlayer { get => canTargetPlayer; }
    public bool CanTargetEnemies { get => canTargetEnemies; }

    public bool TargetAllEnemies { get => targetAllEnemies; }
}