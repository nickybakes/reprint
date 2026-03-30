using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ValueInput
{
    [SerializeField] private ValueType type;

    [SerializeField] private int baseValue;

    [SerializeField] private int maxValue;

    public ValueInput(int _baseValue)
    {
        type = ValueType.Single;
        baseValue = _baseValue;
    }

    public int GetValue()
    {
        if (type == ValueType.Range)
        {
            return Random.Range(baseValue, maxValue + 1);
        }

        return baseValue;
    }

    public int GetMinValue()
    {
        return baseValue;
    }

    public int GetMaxValue()
    {
        if (type == ValueType.Range)
        {
            return maxValue;
        }

        return baseValue;
    }
}

public enum ValueType
{
    Single,
    Range
}