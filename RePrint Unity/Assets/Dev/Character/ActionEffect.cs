using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ActionEffect
{
    public ActionEffectType type;
    public ValueInput valueInput;
    public ChainModifier chainModifier;
}

public enum ActionEffectType
{
    DoDamage,
    GainDodge,
    GainChain,
    ApplyStatusEffect,
    CooldownAction
}