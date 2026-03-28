using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/Ability Data")]
public class AbilityData : ScriptableObject
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


    [field: SerializeField, Header("Ability Effect Lists")] public BetterEditorList<AbilityEffect> AbilityEffectsOverclock0 { get; private set; }
    [field: SerializeField] public BetterEditorList<AbilityEffect> AbilityEffectsOverclock1 { get; private set; }
    [field: SerializeField] public BetterEditorList<AbilityEffect> AbilityEffectsOverclock2 { get; private set; }
    [field: SerializeField] public BetterEditorList<AbilityEffect> AbilityEffectsOverclock3 { get; private set; }
    [field: SerializeField] public BetterEditorList<AbilityEffect> AbilityEffectsOverclock4 { get; private set; }
}