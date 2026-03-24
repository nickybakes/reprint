using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Controls the transforms of Floating Figures in a group.
/// </summary>
public class FloatingFigureGroup : MonoBehaviour
{
    /// <summary>
    /// The spline track for figures to be on.
    /// </summary>
    [SerializeField] protected SplineContainer splineContainer;

    /// <summary>
    /// How to align the figures along the group.
    /// </summary>
    [SerializeField] protected TextAlignment alignment = TextAlignment.Center;

    /// <summary>
    /// The normalized size of regular figures in the group.
    /// </summary>
    [SerializeField][Range(0.0f, 1.0f)] protected float figureSizeNormalized;

    /// <summary>
    /// The normalized size of big (selected) figures in the group.
    /// </summary>
    [SerializeField][Range(0.0f, 1.0f)] protected float bigFigureSizeNormalized;

    /// <summary>
    /// The first bound of the lerped rotation to apply to figures in the group.
    /// </summary>
    [SerializeField] protected Vector3 rotationFirst;

    /// <summary>
    /// The last bound of the lerped rotation to apply to figures in the group.
    /// </summary>
    [SerializeField] protected Vector3 rotationLast;

    /// <summary>
    /// The move speed of floating figure spaces.
    /// </summary>
    [SerializeField] protected float figureSpaceMoveSpeed = 4f;

    /// <summary>
    /// The grow speed of floating figure spaces.
    /// </summary>
    [SerializeField] protected float figureSpaceGrowSpeed = 4f;

    /// <summary>
    /// Whether figures in this group should inherit the rotation of the Figure Group and its parents.
    /// </summary>
    [SerializeField] protected bool figuresInheritRotation = true;

    /// <summary>
    /// Whether figures in this group should inherit the scale of the Figure Group and its parents.
    /// </summary>
    [SerializeField] protected bool figuresInheritScale = true;

    /// <summary>
    /// The list of floating figure spaces in this Figure Group.
    /// </summary>
    protected List<FloatingFigureSpace> figureSpaces;

    /// <summary>
    /// The number of figures in this group.
    /// </summary>
    protected int numFiguresInGroup;

    /// <summary>
    /// Initialize the Figure Group spaces list.
    /// </summary>
    void Awake()
    {
        figureSpaces = new List<FloatingFigureSpace>(5);
    }

    /// <summary>
    /// Update the Figure Group spaces and the transforms of the figure figures in this group.
    /// </summary>
    void Update()
    {
        UpdateSpaces();
        UpdateFigureTransforms();
    }

    /// <summary>
    /// Add a figure figure to this group and reposition figure figures in this group if needed.
    /// </summary>
    /// <param name="figure">The figure figure to add.</param>
    /// <param name="indexPosition">Optional specified index of where in this group the figure should be at.</param>
    public void AddFigureToGroup(FloatingFigure figure, int indexPosition = -1)
    {
        // If not specified index position, put figure at the end.
        if (indexPosition == -1)
            indexPosition = numFiguresInGroup;

        numFiguresInGroup++;

        // Try to find a Figure Space thats not being used, if not create a new one.
        FloatingFigureSpace newSpace = null;
        int newSpaceIndexInArray = -1;
        for (int i = 0; i < figureSpaces.Count; i++)
        {
            if (figureSpaces[i].figureRemoved)
            {
                newSpace = figureSpaces[i];
                newSpaceIndexInArray = i;
                figureSpaces[i].SetupFigure(figure, indexPosition);
                break;
            }
        }
        if (newSpace == null)
        {
            newSpace = new FloatingFigureSpace(figure, indexPosition);
            figureSpaces.Add(newSpace);
            newSpaceIndexInArray = figureSpaces.Count - 1;
        }

        for (int i = 0; i < figureSpaces.Count; i++)
        {
            if (i != newSpaceIndexInArray && !figureSpaces[i].figureRemoved)
                figureSpaces[i].IncrementGoalPositionIfGreaterThanIndex(indexPosition - 1);
        }
    }

    public void UpdateSpaces()
    {
        for (int i = 0; i < figureSpaces.Count; i++)
        {
            figureSpaces[i].Update(figureSpaceGrowSpeed, figureSpaceMoveSpeed);
        }
    }

    /// <summary>
    /// Calculate transforms for figures in the group.
    /// </summary>
    public void UpdateFigureTransforms()
    {
        // Calculate overall space and normalized scale of figures in the figure.
        float totalSpaceSize = 0;
        for (int i = 0; i < figureSpaces.Count; i++)
        {
            FloatingFigureSpace currentSpace = figureSpaces[i];
            totalSpaceSize += currentSpace.SpaceSize;
        }

        float scale = Math.Min(1f / (figureSizeNormalized * totalSpaceSize), 1f);
        float scaledFigureSizeNormalized = figureSizeNormalized * scale;
        float scaledBigFigureSizeNormalized = bigFigureSizeNormalized * scale;

        // Calculate offsets of figures based on their sizes.
        for (int i = 0; i < figureSpaces.Count; i++)
        {
            FloatingFigureSpace currentSpace = figureSpaces[i];
            currentSpace.positionOffset = 0;

            for (int j = 0; j < figureSpaces.Count; j++)
            {
                FloatingFigureSpace compareSpace = figureSpaces[j];
                float offset = GetFigureOffsetSizeNormalized(j, scaledFigureSizeNormalized, scaledBigFigureSizeNormalized) * compareSpace.SpaceSize * .5f;
                offset *= Mathf.Clamp(currentSpace.SpaceIndexPosition - compareSpace.SpaceIndexPosition, -1, 1);
                currentSpace.positionOffset += offset;
            }
        }

        // Calculate starting point for positioning figures from.
        float startPoint = 0;
        if (alignment == TextAlignment.Center)
        {
            startPoint = (scaledFigureSizeNormalized * .5f) + .5f - (totalSpaceSize * .5f * scaledFigureSizeNormalized);
        }
        else if (alignment == TextAlignment.Right)
        {
            startPoint = 1.0f - ((totalSpaceSize - 1.0f) * scaledFigureSizeNormalized);
        }


        // Combine starting point and offset to position the figures.
        for (int i = 0; i < figureSpaces.Count; i++)
        {
            FloatingFigureSpace currentSpace = figureSpaces[i];
            if (currentSpace.figureRemoved)
                continue;

            float point = startPoint + (currentSpace.SpaceIndexPosition * scaledFigureSizeNormalized) + currentSpace.positionOffset;

            Vector3 position = splineContainer.Spline.EvaluatePosition(point);
            currentSpace.figure.SetGoalTransform(position, Quaternion.Euler(Vector3.Lerp(rotationFirst, rotationLast, point)), Vector3.one);
            currentSpace.figure.ApplyTransformParentToGoalTransform(transform, figuresInheritRotation, figuresInheritScale);
        }
    }

    /// <summary>
    /// Remove a Figure from this Group and reposition the rest of the Figures.
    /// </summary>
    /// <param name="figure"></param>
    public void RemoveFigureFromGroup(FloatingFigure figure)
    {
        int removeIndex = numFiguresInGroup;

        for (int i = 0; i < figureSpaces.Count; i++)
        {
            if (figureSpaces[i].figure == figure && !figureSpaces[i].figureRemoved)
            {
                figureSpaces[i].figureRemoved = true;
                figureSpaces[i].SetGoalSize(0);
                numFiguresInGroup--;
                removeIndex = (int)figureSpaces[i].SpaceIndexPositionGoal;
                break;
            }
        }

        for (int i = 0; i < figureSpaces.Count; i++)
        {
            figureSpaces[i].DecrementGoalPositionIfGreaterThanIndex(removeIndex);
        }
    }

    /// <summary>
    /// Get the normalized figure size by lerping its transition amount and the figure space size.
    /// </summary>
    /// <param name="index">The index of the figure figure.</param>
    /// <param name="scaledFigureSizeNormalized">The scale normalized size of a regular figure.</param>
    /// <param name="scaledBigFigureSizeNormalized">The scale normalized size of a big (selected) figure.</param>
    /// <returns>The calculated normalized size of a figure figure.</returns>
    protected float GetFigureOffsetSizeNormalized(int index, float scaledFigureSizeNormalized, float scaledBigFigureSizeNormalized)
    {
        float sizeTransition = 0;
        if (!figureSpaces[index].figureRemoved)
            sizeTransition = figureSpaces[index].figure.SizeTransition;
        return (scaledBigFigureSizeNormalized - scaledFigureSizeNormalized) * sizeTransition;
    }
}


