using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyBehavior
{
    [SerializeField] private BetterEditorList<GameCondition> conditions;
    [SerializeField] private BetterEditorList<EnemyAbilityWeight> abilityWeights;
    [SerializeField] private bool breakOutIfConditionsAreTrue;

    public List<GameCondition> Conditions { get => conditions.List; }
    public List<EnemyAbilityWeight> AbilityWeights { get => abilityWeights.List; }
    public bool BreakOutIfConditionsAreTrue { get => breakOutIfConditionsAreTrue; }

}
