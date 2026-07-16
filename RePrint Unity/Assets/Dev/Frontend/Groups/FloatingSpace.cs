using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// A floating position and size within a Group.
/// </summary>
public class FloatingSpace
{
    /// <summary>
    /// Current extra offset to add to the position.
    /// </summary>
    public float positionOffset;

    /// <summary>
    /// The current floating index in the group that this space is at.
    /// </summary>
    public float SpaceIndexPosition { get => spaceIndexPosition; }

    /// <summary>
    /// The index in the group that this space is trying to be at.
    /// </summary>
    public float SpaceIndexPositionGoal { get => spaceIndexPositionGoal; }

    /// <summary>
    /// The normalized size of this space, from 0 to 1.
    /// </summary>
    public float SpaceSize { get => spaceSize; }

    public bool fakeSpace;

    /// <summary>
    /// The current floating index in the group that this space is at.
    /// </summary>
    protected float spaceIndexPosition;

    /// <summary>
    /// The index in the group that this space is trying to be at.
    /// </summary>
    protected float spaceIndexPositionGoal;

    /// <summary>
    /// The normalized size of this space, from 0 to 1.
    /// </summary>
    protected float spaceSize;

    /// <summary>
    /// The normalized size of this space, from 0 to 1, this this space is growing/shrinking towards.
    /// </summary>
    protected float spaceSizeGoal;

    /// <summary>
    /// The direction that this space's index position is moving towards, if at all.
    /// </summary>
    protected float moveDirection;

    /// <summary>
    /// Whether this space's size is growing, shrinking, or not doing either.
    /// </summary>
    protected float growDirection;

    /// <summary>
    /// Sets the initial goals of this space.
    /// </summary>
    /// <param name="_spaceIndexPosition">The index in the group that this space is trying to be at.</param>
    public void Setup(float _spaceIndexPosition)
    {
        SetGoalSize(1);
        spaceIndexPosition = _spaceIndexPosition;
        spaceIndexPositionGoal = _spaceIndexPosition;
        moveDirection = 0;
    }

    /// <summary>
    /// Update the position and size of this space.
    /// </summary>
    /// <param name="growSpeed">The speed that this space should grow at.</param>
    /// <param name="moveSpeed">The speed that this space should move at.</param>
    public void Update(float growSpeed, float moveSpeed)
    {
        if (growDirection != 0)
        {
            spaceSize += growDirection * growSpeed * Time.deltaTime;

            if ((growDirection > 0 && spaceSize >= spaceSizeGoal) || (growDirection < 0 && spaceSize <= spaceSizeGoal))
            {
                spaceSize = spaceSizeGoal;
                growDirection = 0;
            }
        }

        if (moveDirection != 0)
        {
            spaceIndexPosition += moveDirection * moveSpeed * Time.deltaTime;

            if ((moveDirection > 0 && spaceIndexPosition >= spaceIndexPositionGoal) || (moveDirection < 0 && spaceIndexPosition <= spaceIndexPositionGoal))
            {
                spaceIndexPosition = spaceIndexPositionGoal;
                moveDirection = 0;
            }
        }

    }

    /// <summary>
    /// If the goal position is greater than a specified index, decrement it.
    /// </summary>
    /// <param name="index">The index threshold.</param>
    /// <returns>Whether the condition was passed.</returns>
    public bool DecrementGoalPositionIfGreaterThanIndex(int index)
    {
        bool passed = spaceIndexPositionGoal > index;

        if (passed)
            SetGoalPosition(spaceIndexPositionGoal - 1);

        return passed;
    }

    /// <summary>
    /// If the goal position is greater than a specified index, increment it.
    /// </summary>
    /// <param name="index">The index threshold.</param>
    /// <returns>Whether the condition was passed.</returns>
    public bool IncrementGoalPositionIfGreaterThanIndex(int index)
    {
        bool passed = spaceIndexPositionGoal > index;

        if (passed)
            SetGoalPosition(spaceIndexPositionGoal + 1);

        return passed;
    }

    /// <summary>
    /// Sets the goal index position and makes the space move towards that.
    /// </summary>
    /// <param name="newPosition">The new goal index position</param>
    public void SetGoalPosition(float newPosition)
    {
        spaceIndexPositionGoal = newPosition;
        if (spaceIndexPosition != spaceIndexPositionGoal)
            moveDirection = Math.Sign(spaceIndexPositionGoal - spaceIndexPosition);
    }

    /// <summary>
    /// Sets the goal size and makes the space grow/shrink towards that.
    /// </summary>
    /// <param name="newPosition">The new goal size</param>
    public void SetGoalSize(float newSize)
    {
        spaceSizeGoal = newSize;
        if (spaceSize != spaceSizeGoal)
            growDirection = Math.Sign(spaceSizeGoal - spaceSize);
    }
}


