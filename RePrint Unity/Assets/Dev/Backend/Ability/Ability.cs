using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Ability
{
    public static int MAX_OVERCLOCK = 4;

    public string Name { get { return baseData.Name; } }

    private AbilityData baseData;
    private List<List<AbilityEffect>> abilityEffectsForEachOverclock;
    private List<AbilityRules> abilityRulesForEachOverclock;

    public Ability(AbilityData data)
    {
        baseData = data;

        abilityEffectsForEachOverclock = new List<List<AbilityEffect>>(MAX_OVERCLOCK);
        abilityRulesForEachOverclock = new List<AbilityRules>(MAX_OVERCLOCK);

        abilityEffectsForEachOverclock.Add(baseData.AbilityEffectsOverclock0.List);
        abilityEffectsForEachOverclock.Add(baseData.AbilityEffectsOverclock1.List);
        abilityEffectsForEachOverclock.Add(baseData.AbilityEffectsOverclock2.List);
        abilityEffectsForEachOverclock.Add(baseData.AbilityEffectsOverclock3.List);
        abilityEffectsForEachOverclock.Add(baseData.AbilityEffectsOverclock4.List);

        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock0);
        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock1);
        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock2);
        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock3);
        abilityRulesForEachOverclock.Add(baseData.AbilityRulesOverclock4);
    }

    public List<AbilityEffect> GetAbilityEffects(int overclock)
    {
        return abilityEffectsForEachOverclock[overclock];
    }

    public AbilityRules GetAbilityRules(int overclock)
    {
        return abilityRulesForEachOverclock[overclock];
    }
}