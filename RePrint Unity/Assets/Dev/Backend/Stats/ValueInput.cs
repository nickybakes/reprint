using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ValueInput
{
    [SerializeField] private ValueType type;

    [SerializeField] private float baseValue;

    [SerializeField] private float maxValue;
    [SerializeField] private bool floatMode;

    public ValueInput(int _baseValue)
    {
        type = ValueType.Single;
        baseValue = _baseValue;
    }

    public float GetValue()
    {
        if (type == ValueType.Range)
        {
            return Random.Range(baseValue, maxValue + 1);
        }

        return baseValue;
    }

    public float GetMinValue()
    {
        return baseValue;
    }

    public float GetMaxValue()
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