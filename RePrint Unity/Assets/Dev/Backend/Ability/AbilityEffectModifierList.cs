using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityEffectModifierList
{
    [SerializeField] private List<AbilityEffectModifier> modifiers;

    public List<AbilityEffectModifier> Modifiers { get => modifiers; }
}
