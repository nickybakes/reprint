using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Arithmetic
{
    [SerializeField] private MathType mathType;
    [SerializeField] private GameValueType gameValueType;

    /// <summary>
    /// When false, the equation has the Incoming Value come after the Value in the equation.
    /// For example, in Subtract, it would be Value - Incoming. If you toggle this on, then it would be Incoming - Value
    /// </summary>
    [SerializeField] private bool invertEquation;

    [SerializeField] private bool clamp;

    [SerializeField] private float minClamp;

    [SerializeField] private float maxClamp;

    public GameValueType GameValueType { get => gameValueType; }



    public float CalculateSolution(float _value, float _incomingValue)
    {
        float a = _value;
        float b = _incomingValue;

        if (invertEquation)
        {
            a = _incomingValue;
            b = _value;
        }

        float finalValue = 0;

        switch (mathType)
        {
            case MathType.Add:
                finalValue = a + b;
                break;
            case MathType.Subtract:
                finalValue = a - b;
                break;
            case MathType.Multiply:
                finalValue = a * b;
                break;
            case MathType.Divide:
                finalValue = a / b;
                break;
        }

        if (clamp)
        {
            return Math.Clamp(finalValue, minClamp, maxClamp);
        }

        return finalValue;
    }
}

public enum MathType
{
    Add,
    Subtract,
    Multiply,
    Divide
}