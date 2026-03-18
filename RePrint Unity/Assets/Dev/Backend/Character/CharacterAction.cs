using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CharacterAction
{
    private CharacterActionData baseData;

    public string Name { get { return baseData.name; } }

    public CharacterAction(CharacterActionData data)
    {
        baseData = data;
    }
}