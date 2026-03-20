using System;
using Unity.VisualScripting;
using UnityEngine;

public class AbilitySelection
{
    public Ability ability;

    public int overclock = 0;

    public Character target;

    public AbilitySelection(Ability _ability)
    {
        ability = _ability;
        target = null;
    }

    public void Overclock()
    {
        overclock = Math.Min(overclock + 1, Ability.MAX_OVERCLOCK);
    }

    public void Underclock()
    {
        overclock = Math.Max(overclock - 1, 0);
    }
}