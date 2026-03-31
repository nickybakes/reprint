using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModBehavior
{
    [SerializeField] private BetterEditorList<GameCondition> conditions;
    [SerializeField] private BetterEditorList<ModEffect> effects;

    public List<GameCondition> Conditions { get => conditions.List; }
    public List<ModEffect> Effects { get => effects.List; }

}
