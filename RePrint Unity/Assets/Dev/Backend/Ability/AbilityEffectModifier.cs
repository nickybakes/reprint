using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityEffectModifier
{
    public MathType mathType;
    public ModifierIncomingValue incomingValue;

    /// <summary>
    /// When false, the equation has the Incoming Value come after the Value in the equation.
    /// For example, in Subtract, it would be Value - Incoming. If you check this, then it would be Incoming - Value
    /// </summary>
    public bool invertEquation;

    public bool clamp;

    public int minClamp;

    public int maxClamp;
}

public enum ModifierIncomingValue
{
    Chain,
    NumberOfEnemies
}

public enum MathType
{
    Add,
    Subtract,
    Multiply,
    Divide
}