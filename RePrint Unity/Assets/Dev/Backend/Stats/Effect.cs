using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Effect
{
    [SerializeField] protected BetterEditorList<EffectApplication> applicationModes;

    public List<EffectApplication> ApplicationModes { get => applicationModes.List; }

    public abstract int GetAmount(GameValues gameValues, bool getMinimum = false, bool getMaximum = false);
}