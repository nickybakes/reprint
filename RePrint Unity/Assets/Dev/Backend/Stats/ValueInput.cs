using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ValueInput
{
    public ValueType type;

    public int baseValue;

    public int maxValue;

    public int GetValue()
    {
        if (type == ValueType.Range)
        {
            return Random.Range(baseValue, maxValue + 1);
        }

        return baseValue;
    }
}

public enum ValueType
{
    Single,
    Range
}