using System;
using UnityEngine;

[Serializable]
public class ChainModifier
{
    public ChainModifierType type;

    /// <summary>
    /// When false, the equation has Chain come after the Value in the equaiton.
    /// For example, in Subtract, it would be Value - Chain. If you check this, then it would be Chain - Value
    /// </summary>
    public bool invertEquation;

    public bool clamp;

    public int minClamp;

    public int maxClamp;
}

public enum ChainModifierType
{
    None,
    Add,
    Subtract,
    Multiply,
    Divide
}