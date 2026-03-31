using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAbilityData", menuName = "Scriptable Objects/Enemy Ability Data")]
public class EnemyAbilityData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public EnemyIntent Intent { get; private set; }

    [field: SerializeField] public BetterEditorList<AbilityBehavior> Behaviors { get; private set; }
}