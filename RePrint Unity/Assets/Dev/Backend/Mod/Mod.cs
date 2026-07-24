using System.Collections.Generic;
using UnityEngine;

public class Mod
{
    public ModProfile Profile { get { return baseData.Profile; } }

    public string Name { get { return baseData.Profile.Name; } }
    public string Description { get { return baseData.Profile.Description; } }
    public List<ModBehavior> Behaviors { get { return baseData.ModBehaviors.List; } }
    public string DebugName { get { return baseData.Name; } }

    protected ModData baseData;

    public AbilitySelection[] internalAbilitySelectionStorage;

    public Mod(ModData data)
    {
        baseData = data;
        internalAbilitySelectionStorage = new AbilitySelection[5];
    }

    public List<ModEffect> GetModEffects(List<bool> passingBehaviors)
    {
        List<ModEffect> effects = new List<ModEffect>();
        for (int i = 0; i < passingBehaviors.Count; i++)
        {
            if (passingBehaviors[i])
            {
                effects.AddRange(Behaviors[i].Effects);
                if (Behaviors[i].BreakOutIfConditionsAreTrue)
                {
                    return effects;
                }
            }
        }
        return effects;
    }

}
