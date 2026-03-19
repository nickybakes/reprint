using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Ability
{
    private AbilityData baseData;

    public string Name { get { return baseData.name; } }

    public Ability(AbilityData data)
    {
        baseData = data;
    }
}