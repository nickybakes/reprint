using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterActionData", menuName = "Scriptable Objects/CharacterActionData")]
public class CharacterActionData : ScriptableObject
{
    public new string name;

    [TextArea]
    public string description;

    public int baseActionPointCost;

    [Header("Action Rules")]
    public ActionRules actionRulesOverclock0;
    public ActionRules actionRulesOverclock1;
    public ActionRules actionRulesOverclock2;
    public ActionRules actionRulesOverclock3;
    public ActionRules actionRulesOverclock4;

    [Header("Action Effect Lists")]
    public ActionEffectList actionEffectsOverclock0;
    public ActionEffectList actionEffectsOverclock1;
    public ActionEffectList actionEffectsOverclock2;
    public ActionEffectList actionEffectsOverclock3;
    public ActionEffectList actionEffectsOverclock4;
}