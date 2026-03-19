using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityEffectList
{
    [SerializeField] private List<AbilityEffect> abilityEffects;

    public List<AbilityEffect> AbilityEffects { get => abilityEffects; }
}
