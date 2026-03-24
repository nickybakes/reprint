using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// A floating position and size within a Group.
/// </summary>
public class FloatingFigureSpace : FloatingSpace
{
    /// <summary>
    /// The figure that is tied to this position.
    /// </summary>
    public FloatingFigure figure;

    /// <summary>
    /// If the figure has been removed from the group/this space.
    /// </summary>
    public bool figureRemoved;

    /// <summary>
    /// Contructor that stores the figure and sets up the space.
    /// </summary>
    /// <param name="_figure">The figure that is tied to this position.</param>
    /// <param name="_spaceIndexPosition">The index in the group that this space is trying to be at.</param>
    public FloatingFigureSpace(FloatingFigure _figure, float _spaceIndexPosition)
    {
        SetupFigure(_figure, _spaceIndexPosition);
    }

    /// <summary>
    /// Sets the initial goals of this space.
    /// </summary>
    /// <param name="_figure">The figure that is tied to this position.</param>
    /// <param name="_spaceIndexPosition">The index in the group that this space is trying to be at.</param>
    public void SetupFigure(FloatingFigure _figure, float _spaceIndexPosition)
    {
        figure = _figure;
        figureRemoved = false;
        Setup(_spaceIndexPosition);
    }
}


