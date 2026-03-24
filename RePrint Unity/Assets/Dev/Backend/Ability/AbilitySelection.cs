using System;
using Unity.VisualScripting;
using UnityEngine;

public class AbilitySelection
{
    public Ability Ability { get; private set; }

    public int Overclock { get; private set; }

    public Character Target { get; private set; }

    public bool TargetIsSet { get; private set; }

    public AbilitySelection(Ability _ability)
    {
        Ability = _ability;
        Target = null;
        TargetIsSet = false;
    }

    public void SetAbility(Ability _ability)
    {
        Ability = _ability;
        Overclock = 0;
        TargetIsSet = false;
    }

    public void SetTarget(Character _Target)
    {
        Target = _Target;
        TargetIsSet = true;
    }

    public void UnsetTarget()
    {
        Target = null;
        TargetIsSet = false;
    }

    public void IncreaseOverclock()
    {
        Overclock = Math.Min(Overclock + 1, Ability.MAX_OVERCLOCK);
    }

    public void DecreaseOverclock()
    {
        Overclock = Math.Max(Overclock - 1, 0);
    }
}