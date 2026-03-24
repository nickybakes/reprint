using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// A floating position and size within a Group.
/// </summary>
public class FloatingDisplaySpace : FloatingSpace
{

    /// <summary>
    /// The display that is tied to this position.
    /// </summary>
    public FloatingDisplay display;

    /// <summary>
    /// If the display has been removed from the group/this space.
    /// </summary>
    public bool displayRemoved;

    /// <summary>
    /// Contructor that stores the display and sets up the space.
    /// </summary>
    /// <param name="_display">The display that is tied to this position.</param>
    /// <param name="_spaceIndexPosition">The index in the group that this space is trying to be at.</param>
    public FloatingDisplaySpace(FloatingDisplay _display, float _spaceIndexPosition)
    {
        SetupDisplay(_display, _spaceIndexPosition);
    }

    /// <summary>
    /// Sets the initial goals of this display space.
    /// </summary>
    /// <param name="_display">The display that is tied to this position.</param>
    /// <param name="_spaceIndexPosition">The index in the group that this space is trying to be at.</param>
    public void SetupDisplay(FloatingDisplay _display, float _spaceIndexPosition)
    {
        display = _display;
        displayRemoved = false;
        Setup(_spaceIndexPosition);
    }
}


