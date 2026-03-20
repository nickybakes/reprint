using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModBehavior
{
    [SerializeField] private BetterEditorList<ModCondition> conditions;
    [SerializeField] private BetterEditorList<ModEffect> effects;

    public List<ModCondition> Conditions { get => conditions.List; }
    public List<ModEffect> Effects { get => effects.List; }

}
