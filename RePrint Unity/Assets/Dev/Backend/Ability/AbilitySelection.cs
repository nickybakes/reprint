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
}