using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbilityData", menuName = "Scriptable Objects/Player Ability Data")]
public class PlayerAbilityData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public AbilityType Type { get; private set; }

    [field: SerializeField, Header("Ability Rules")] public AbilityRules AbilityRulesOverclock0 { get; private set; }
    [field: SerializeField] public AbilityRules AbilityRulesOverclock1 { get; private set; }
    [field: SerializeField] public AbilityRules AbilityRulesOverclock2 { get; private set; }
    [field: SerializeField] public AbilityRules AbilityRulesOverclock3 { get; private set; }
    [field: SerializeField] public AbilityRules AbilityRulesOverclock4 { get; private set; }

    [field: SerializeField, Header("Ability Behaviors")] public BetterEditorList<AbilityBehavior> AbilityOverclock0Behaviors { get; private set; }
    [field: SerializeField] public BetterEditorList<AbilityBehavior> AbilityOverclock1Behaviors { get; private set; }
    [field: SerializeField] public BetterEditorList<AbilityBehavior> AbilityOverclock2Behaviors { get; private set; }
    [field: SerializeField] public BetterEditorList<AbilityBehavior> AbilityOverclock3Behaviors { get; private set; }
    [field: SerializeField] public BetterEditorList<AbilityBehavior> AbilityOverclock4Behaviors { get; private set; }
}