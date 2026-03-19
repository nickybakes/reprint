using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/Ability Data")]
public class AbilityData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    [field: SerializeField] public int BaseAPCost { get; private set; }

    [field: SerializeField, Header("Ability Rules")] public AbilityRules AbilityRulesOverclock0 { get; private set; }
    [field: SerializeField] public AbilityRules AbilityRulesOverclock1 { get; private set; }
    [field: SerializeField] public AbilityRules AbilityRulesOverclock2 { get; private set; }
    [field: SerializeField] public AbilityRules AbilityRulesOverclock3 { get; private set; }
    [field: SerializeField] public AbilityRules AbilityRulesOverclock4 { get; private set; }

    [field: SerializeField, Header("Ability Effect Lists")] public AbilityEffectList AbilityEffectsOverclock0 { get; private set; }
    [field: SerializeField] public AbilityEffectList AbilityEffectsOverclock1 { get; private set; }
    [field: SerializeField] public AbilityEffectList AbilityEffectsOverclock2 { get; private set; }
    [field: SerializeField] public AbilityEffectList AbilityEffectsOverclock3 { get; private set; }
    [field: SerializeField] public AbilityEffectList AbilityEffectsOverclock4 { get; private set; }
}