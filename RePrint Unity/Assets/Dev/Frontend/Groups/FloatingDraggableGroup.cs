using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class FloatingDraggableGroup : FloatingDisplayGroup
{
    [SerializeField] protected FloatingDisplay fakeFloatingDisplay;
    [SerializeField] protected FloatingDisplayGroup invisDuplicateGroup;


    [SerializeField] protected float dragConnectDistance = 100;
    public float DragConnectDistance { get => dragConnectDistance; }


    /// <summary>
    /// The event to call when the user starts dragging an element in this group.
    /// </summary>
    [SerializeField] protected UnityEvent<int, FloatingDraggableDisplay> startDragEvent;

    public UnityEvent<int, FloatingDraggableDisplay> StartDragEvent { get => startDragEvent; }

    [SerializeField] protected UnityEvent<int, FloatingDraggableDisplay> stopDragEvent;

    public UnityEvent<int, FloatingDraggableDisplay> StopDragEvent { get => stopDragEvent; }

    protected int indexToInsertTo;

    public int IndexToInsertTo { get => indexToInsertTo; }

    private FloatingDisplaySpace fakeSpace;

    private float3 nearestPointOnSpline;
    private Vector3 localSplinePoint;

    private float nearestNormalizedPoint = 0;

    private int tempRemovedDisplayAmount;

    private Dictionary<FloatingDisplaySpace, float> storedSpacePositions;

    private Vector3 mousePosition;

    public void AddDraggableToGroup(FloatingDraggableDisplay display, int indexPosition = -1)
    {
        if (indexPosition > numDisplaysInGroup)
        {
            indexPosition = -1;
        }
        display.SetGroup(this, AddDisplayToGroup(display, indexPosition));
    }

    public void TempRemoveDraggableFromGroup(FloatingDraggableDisplay display)
    {
        tempRemovedDisplayAmount++;
        RemoveDisplayFromGroup(display);
    }

    public void ResetTempRemovedDraggableAmount()
    {
        tempRemovedDisplayAmount = 0;
    }

    public float TestDistanceToSpline(Vector3 pos)
    {
        mousePosition = pos;
        localSplinePoint = splineContainer.transform.InverseTransformPoint(pos);
        float distance = SplineUtility.GetNearestPoint(splineContainer.Spline, localSplinePoint, out nearestPointOnSpline, out nearestNormalizedPoint);

        return distance;
    }

    public void EnableDragConnection()
    {
        int closestDisplaySpaceIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < invisDuplicateGroup.displaySpaces.Count; i++)
        {
            float distance = Vector2.Distance(mousePosition, invisDuplicateGroup.displaySpaces[i].display.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDisplaySpaceIndex = i;
            }
        }

        fakeSpace.SetGoalSize(1);

        indexToInsertTo = closestDisplaySpaceIndex;
        fakeSpace.SetGoalPosition(closestDisplaySpaceIndex);
        foreach (FloatingDisplaySpace displaySpace in storedSpacePositions.Keys)
        {
            if (storedSpacePositions[displaySpace] >= closestDisplaySpaceIndex)
            {
                displaySpace.SetGoalPosition(storedSpacePositions[displaySpace] + 1f);
            }
            else
            {
                displaySpace.SetGoalPosition(storedSpacePositions[displaySpace]);
            }
        }
    }

    public void DisableDragConnection()
    {
        fakeSpace.SetGoalSize(0);
        foreach (FloatingDisplaySpace displaySpace in storedSpacePositions.Keys)
        {
            displaySpace.SetGoalPosition(storedSpacePositions[displaySpace]);
        }
    }

    public void StartDragMode()
    {
        CopyGroupProperties();

        storedSpacePositions = new Dictionary<FloatingDisplaySpace, float>(displaySpaces.Count - 1);
        Transform newParent = transform.parent;
        foreach (FloatingDisplaySpace displaySpace in displaySpaces)
        {
            if (!displaySpace.displayRemoved && !displaySpace.fakeSpace)
            {
                storedSpacePositions.Add(displaySpace, displaySpace.SpaceIndexPositionGoal);

                newParent = displaySpace.display.transform.parent;
                FloatingDisplay newFakeDisplay = Instantiate(fakeFloatingDisplay, newParent);
                invisDuplicateGroup.AddDisplayToGroup(newFakeDisplay);
            }
        }

        FloatingDisplay extraFakeDisplay = Instantiate(fakeFloatingDisplay, newParent);
        invisDuplicateGroup.AddDisplayToGroup(extraFakeDisplay);

    }

    public void StopDragMode()
    {
        DisableDragConnection();
        invisDuplicateGroup.ClearAndDestroyDisplays();
        tempRemovedDisplayAmount = 0;
    }

    public void StartDragFromElement(FloatingDraggableDisplay display)
    {
        startDragEvent.Invoke(display.IndexInGroup, display);
    }

    public void StopDragFromElement(FloatingDraggableDisplay display)
    {
        stopDragEvent.Invoke(display.IndexInGroup, display);
    }

    void Awake()
    {
        SetupRectTransform();
        displaySpaces = new List<FloatingDisplaySpace>(5);
        fakeSpace = new FloatingDisplaySpace(null, 0)
        {
            displayRemoved = true,
            fakeSpace = true
        };
        fakeSpace.SetGoalSize(0);
        displaySpaces.Add(fakeSpace);
        nearestPointOnSpline = new Vector3();
    }

    void Start()
    {
        CopyGroupProperties();
    }

    private void CopyGroupProperties()
    {
        invisDuplicateGroup.splineContainer = splineContainer;
        invisDuplicateGroup.alignment = alignment;
        invisDuplicateGroup.displaySizeNormalized = displaySizeNormalized;
        invisDuplicateGroup.bigDisplaySizeNormalized = bigDisplaySizeNormalized;
        invisDuplicateGroup.displaySpaceMoveSpeed = 1000;
        invisDuplicateGroup.displaySpaceGrowSpeed = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpaces();
        UpdateDisplayTransforms();
    }
}
