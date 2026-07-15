using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModEffect
{
    [SerializeField] private ModEffectType type;
    public ModEffectType Type { get => type; }

    [SerializeField] private BetterEditorList<AbilityEffectApplication> applicationModes;

    public List<AbilityEffectApplication> ApplicationModes { get => applicationModes.List; }

    [SerializeField] private ValueInput valueInput1;
    public ValueInput ValueInput1 { get => valueInput1; }

}

public enum ModEffectType
{
    DoDamage,
    AffectDealingDamage,
    Heal,
    GainChain,


}