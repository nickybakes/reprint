using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ValueInput
{
    [SerializeField] private ValueType type;

    [SerializeField] private int baseValue;

    [SerializeField] private int maxValue;

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