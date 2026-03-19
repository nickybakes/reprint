using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/Ability Data")]
public class AbilityData : ScriptableObject
{
    public new string name;

    [TextArea]
    public string description;

    public int baseAPCost;

    [Header("Ability Rules")]
    public AbilityRules abilityRulesOverclock0;
    public AbilityRules abilityRulesOverclock1;
    public AbilityRules abilityRulesOverclock2;
    public AbilityRules abilityRulesOverclock3;
    public AbilityRules abilityRulesOverclock4;

    [Header("Ability Effect Lists")]
    public AbilityEffectList abilityEffectsOverclock0;
    public AbilityEffectList abilityEffectsOverclock1;
    public AbilityEffectList abilityEffectsOverclock2;
    public AbilityEffectList abilityEffectsOverclock3;
    public AbilityEffectList abilityEffectsOverclock4;
}