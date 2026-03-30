using System.Collections.Generic;
using UnityEngine;

public class AbilityStatDisplayGroup : MonoBehaviour
{
    [SerializeField] private AbilityStatDisplay physicalDamageDisplay;
    [SerializeField] private AbilityStatDisplay chainDisplay;
    [SerializeField] private AbilityStatDisplay dodgeDisplay;

    public void Refresh(Ability ability, int overclock, Character activator)
    {
        AbilityStats stats = ability.GetAbilityStats(overclock, activator);

        if (physicalDamageDisplay)
            physicalDamageDisplay.DisplayStat(stats.MinPhysicalDamage, stats.MaxPhysicalDamage);

        if (chainDisplay)
            chainDisplay.DisplayStat(stats.MinChain, stats.MaxChain);

        if (dodgeDisplay)
            dodgeDisplay.DisplayStat(stats.MinDodge, stats.MaxDodge);
    }
}
