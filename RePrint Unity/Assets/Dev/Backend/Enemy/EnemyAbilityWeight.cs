using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAbilityWeight
{
    [SerializeField] private EnemyAbilityData abilityData;
    [SerializeField] private int weight;
    public EnemyAbilityData AbilityData { get => abilityData; }
    public int Weight { get => weight; }

}
