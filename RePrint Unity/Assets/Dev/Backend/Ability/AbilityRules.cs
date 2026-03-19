using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityRules
{
    [SerializeField] private bool targetAllEnemies;
    [SerializeField] private ValueInput numberOfHits;
}