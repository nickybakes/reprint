using System;
using UnityEngine;

[Serializable]
public class ValueInput
{
    public ValueType type;

    public int baseValue;

    public int maxValue;
}

public enum ValueType
{
    Single,
    Range
}