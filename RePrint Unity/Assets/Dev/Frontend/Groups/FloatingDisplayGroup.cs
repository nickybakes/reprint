using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Controls the transforms of Floating Displays in a group.
/// </summary>
public class FloatingDisplayGroup : Display
{
    /// <summary>
    /// The spline track for displays to be on.
    /// </summary>
    [SerializeField] protected SplineContainer splineContainer;

    /// <summary>
    /// How to align the displays along the group.
    /// </summary>
    [SerializeField] protected TextAlignment alignment = TextAlignment.Center;

    /// <summary>
    /// The normalized size of regular displays in the group.
    /// </summary>
    [SerializeField][Range(0.0f, 1.0f)] protected float displaySizeNormalized;

    /// <summary>
    /// The normalized size of big (selected) displays in the group.
    /// </summary>
    [SerializeField][Range(0.0f, 1.0f)] protected float bigDisplaySizeNormalized;

    /// <summary>
    /// The first bound of the lerped rotation to apply to displays in the group.
    /// </summary>
    [SerializeField] protected Vector3 rotationFirst;

    /// <summary>
    /// The last bound of the lerped rotation to apply to displays in the group.
    /// </summary>
    [SerializeField] protected Vector3 rotationLast;

    /// <summary>
    /// The move speed of floating display spaces.
    /// </summary>
    [SerializeField] protected float displaySpaceMoveSpeed = 4f;

    /// <summary>
    /// The grow speed of floating display spaces.
    /// </summary>
    [SerializeField] protected float displaySpaceGrowSpeed = 4f;

    /// <summary>
    /// Whether displays in this group should inherit the rotation of the Display Group and its parents.
    /// </summary>
    [SerializeField] protected bool displaysInheritRotation = true;

    /// <summary>
    /// Whether displays in this group should inherit the scale of the Display Group and its parents.
    /// </summary>
    [SerializeField] protected bool displaysInheritScale = true;

    /// <summary>
    /// The list of floating display spaces in this Display Group.
    /// </summary>
    protected List<FloatingDisplaySpace> displaySpaces;

    /// <summary>
    /// The number of displays in this group.
    /// </summary>
    protected int numDisplaysInGroup;

    /// <summary>
    /// Initialize the Display Group spaces list.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        displaySpaces = new List<FloatingDisplaySpace>(5);
    }

    /// <summary>
    /// Update the Display Group spaces and the transforms of the display displays in this group.
    /// </summary>
    void Update()
    {
        UpdateSpaces();
        UpdateDisplayTransforms();
    }

    /// <summary>
    /// Add a display display to this group and reposition display displays in this group if needed.
    /// </summary>
    /// <param name="display">The display display to add.</param>
    /// <param name="indexPosition">Optional specified index of where in this group the display should be at.</param>
    public void AddDisplayToGroup(FloatingDisplay display, int indexPosition = -1)
    {
        // If not specified index position, put display at the end.
        if (indexPosition == -1)
            indexPosition = numDisplaysInGroup;

        numDisplaysInGroup++;

        // Try to find a Display Space thats not being used, if not create a new one.
        FloatingDisplaySpace newSpace = null;
        int newSpaceIndexInArray = -1;
        for (int i = 0; i < displaySpaces.Count; i++)
        {
            if (displaySpaces[i].displayRemoved)
            {
                newSpace = displaySpaces[i];
                newSpaceIndexInArray = i;
                displaySpaces[i].SetupDisplay(display, indexPosition);
                break;
            }
        }
        if (newSpace == null)
        {
            newSpace = new FloatingDisplaySpace(display, indexPosition);
            displaySpaces.Add(newSpace);
            newSpaceIndexInArray = displaySpaces.Count - 1;
        }

        // Adjust the order in the hierarchy so displays are stacked on top of eachother.
        newSpace.display.transform.SetAsLastSibling();

        for (int i = 0; i < displaySpaces.Count; i++)
        {
            if (i != newSpaceIndexInArray && !displaySpaces[i].displayRemoved && displaySpaces[i].IncrementGoalPositionIfGreaterThanIndex(indexPosition - 1))
                displaySpaces[i].display.transform.SetAsLastSibling();
        }
    }

    public void UpdateSpaces()
    {
        for (int i = 0; i < displaySpaces.Count; i++)
        {
            displaySpaces[i].Update(displaySpaceGrowSpeed, displaySpaceMoveSpeed);
        }
    }

    /// <summary>
    /// Calculate transforms for displays in the group.
    /// </summary>
    public void UpdateDisplayTransforms()
    {
        // Calculate overall space and normalized scale of displays in the display.
        float totalSpaceSize = 0;
        for (int i = 0; i < displaySpaces.Count; i++)
        {
            FloatingDisplaySpace currentSpace = displaySpaces[i];
            totalSpaceSize += currentSpace.SpaceSize;
        }

        float scale = Math.Min(1f / (displaySizeNormalized * totalSpaceSize), 1f);
        float scaledDisplaySizeNormalized = displaySizeNormalized * scale;
        float scaledBigDisplaySizeNormalized = bigDisplaySizeNormalized * scale;

        // Calculate offsets of displays based on their sizes.
        for (int i = 0; i < displaySpaces.Count; i++)
        {
            FloatingDisplaySpace currentSpace = displaySpaces[i];
            currentSpace.positionOffset = 0;

            for (int j = 0; j < displaySpaces.Count; j++)
            {
                FloatingDisplaySpace compareSpace = displaySpaces[j];
                float offset = GetDisplayOffsetSizeNormalized(j, scaledDisplaySizeNormalized, scaledBigDisplaySizeNormalized) * compareSpace.SpaceSize * .5f;
                offset *= Mathf.Clamp(currentSpace.SpaceIndexPosition - compareSpace.SpaceIndexPosition, -1, 1);
                currentSpace.positionOffset += offset;
            }
        }

        // Calculate starting point for positioning displays from.
        float startPoint = 0;
        if (alignment == TextAlignment.Center)
        {
            startPoint = (scaledDisplaySizeNormalized * .5f) + .5f - (totalSpaceSize * .5f * scaledDisplaySizeNormalized);
        }
        else if (alignment == TextAlignment.Right)
        {
            startPoint = 1.0f - ((totalSpaceSize - 1.0f) * scaledDisplaySizeNormalized);
        }


        // Combine starting point and offset to position the displays.
        for (int i = 0; i < displaySpaces.Count; i++)
        {
            FloatingDisplaySpace currentSpace = displaySpaces[i];
            if (currentSpace.displayRemoved)
                continue;

            float point = startPoint + (currentSpace.SpaceIndexPosition * scaledDisplaySizeNormalized) + currentSpace.positionOffset;

            Vector3 position = splineContainer.Spline.EvaluatePosition(point);
            position.z = 0;
            currentSpace.display.SetGoalTransform(position, Quaternion.Euler(Vector3.Lerp(rotationFirst, rotationLast, point)), Vector3.one);
            currentSpace.display.ApplyTransformParentToGoalTransform(rectTransform, displaysInheritRotation, displaysInheritScale);
        }
    }

    /// <summary>
    /// Remove a Display from this Group and reposition the rest of the Displays.
    /// </summary>
    /// <param name="display"></param>
    public void RemoveDisplayFromGroup(FloatingDisplay display)
    {
        int removeIndex = numDisplaysInGroup;

        for (int i = 0; i < displaySpaces.Count; i++)
        {
            if (displaySpaces[i].display == display && !displaySpaces[i].displayRemoved)
            {
                displaySpaces[i].displayRemoved = true;
                displaySpaces[i].SetGoalSize(0);
                numDisplaysInGroup--;
                removeIndex = (int)displaySpaces[i].SpaceIndexPositionGoal;
                break;
            }
        }

        for (int i = 0; i < displaySpaces.Count; i++)
        {
            displaySpaces[i].DecrementGoalPositionIfGreaterThanIndex(removeIndex);
        }
    }

    /// <summary>
    /// Get the normalized display size by lerping its transition amount and the display space size.
    /// </summary>
    /// <param name="index">The index of the display display.</param>
    /// <param name="scaledDisplaySizeNormalized">The scale normalized size of a regular display.</param>
    /// <param name="scaledBigDisplaySizeNormalized">The scale normalized size of a big (selected) display.</param>
    /// <returns>The calculated normalized size of a display display.</returns>
    protected float GetDisplayOffsetSizeNormalized(int index, float scaledDisplaySizeNormalized, float scaledBigDisplaySizeNormalized)
    {
        float sizeTransition = 0;
        if (!displaySpaces[index].displayRemoved)
            sizeTransition = displaySpaces[index].display.SizeTransition;
        return (scaledBigDisplaySizeNormalized - scaledDisplaySizeNormalized) * sizeTransition;
    }
}


