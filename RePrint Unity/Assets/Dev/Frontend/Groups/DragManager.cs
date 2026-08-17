using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragManager : MonoBehaviour
{

    public static DragManager instance;

    public List<FloatingDraggableGroup> groups;

    [SerializeField] private FloatingDisplayGroup cursorGroup;
    [SerializeField] private TravelingDisplay cursorDisplay;

    private List<FloatingDraggableDisplay> displaysBeingDragged;

    private bool isDragging;

    private int groupIndexToInsertIn;

    public FloatingDisplayGroup CursorGroup
    {
        get => cursorGroup;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public void StartDragging(List<FloatingDraggableDisplay> draggables)
    {
        if (!isDragging)
        {
            displaysBeingDragged = draggables;
            foreach (FloatingDraggableDisplay draggable in displaysBeingDragged)
            {
                cursorGroup.AddDisplayToGroup(draggable);
                draggable.Group.TempRemoveDraggableFromGroup(draggable);
            }

            foreach (FloatingDraggableGroup group in groups)
            {
                group.StartDragMode();
            }

            isDragging = true;
            BetterSelectable.ignoreMouseInteractions = true;
        }
    }

    public void StopDragging()
    {
        if (isDragging)
        {
            foreach (FloatingDraggableGroup group in groups)
            {
                group.StopDragMode();
            }

            if (displaysBeingDragged != null)
            {
                if (groupIndexToInsertIn != -1)
                {
                    for (int i = 0; i < displaysBeingDragged.Count; i++)
                    {
                        FloatingDraggableDisplay draggable = displaysBeingDragged[i];
                        groups[groupIndexToInsertIn].AddDraggableToGroup(draggable, groups[groupIndexToInsertIn].IndexToInsertTo + i);
                        cursorGroup.RemoveDisplayFromGroup(draggable);
                    }
                }
                else
                {
                    foreach (FloatingDraggableDisplay draggable in displaysBeingDragged)
                    {
                        draggable.Group.AddDraggableToGroup(draggable, draggable.IndexInGroup);
                        cursorGroup.RemoveDisplayFromGroup(draggable);
                    }
                }
            }

            isDragging = false;
            BetterSelectable.ignoreMouseInteractions = false;
        }
    }

    void Update()
    {
        cursorDisplay.SetGoalTransform(UIView.view.MouseViewPosition, Quaternion.identity, Vector3.one);
        cursorDisplay.UpdateTravel();

        if (isDragging)
        {
            int closestGroupIndex = -1;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < groups.Count; i++)
            {
                float distance = groups[i].TestDistanceToSpline(cursorDisplay.GetRect().position);

                if (distance < groups[i].DragConnectDistance)
                {
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestGroupIndex = i;
                    }
                }
            }

            for (int i = 0; i < groups.Count; i++)
            {
                if (i == closestGroupIndex)
                {
                    groups[i].EnableDragConnection();
                }
                else
                {
                    groups[i].DisableDragConnection();
                }
            }

            groupIndexToInsertIn = closestGroupIndex;
        }
    }
}
