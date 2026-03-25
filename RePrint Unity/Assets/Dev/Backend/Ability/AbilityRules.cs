using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityRules
{
    [SerializeField] private int apCost;
    [SerializeField] private bool targetAllEnemies;
    [SerializeField] private ValueInput numberOfHits;

    public int APCost { get => apCost; }

    public bool TargetAllEnemies { get => targetAllEnemies; }
    public ValueInput NumberOfHits { get => numberOfHits; }
}