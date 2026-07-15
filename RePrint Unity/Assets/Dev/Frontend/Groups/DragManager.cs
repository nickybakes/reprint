using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragManager : MonoBehaviour
{

    public static DragManager instance;

    public List<FloatingDraggableGroup> groups;

    [SerializeField] private FloatingDraggableGroup cursorGroup;
    [SerializeField] private TravelingDisplay cursorDisplay;

    private Vector3 mousePosition;

    public FloatingDraggableGroup CursorGroup
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
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cursorDisplay.SetGoalTransform(UIView.view.MouseViewPosition, Quaternion.identity, Vector3.one);
        cursorDisplay.UpdateTravel();
    }
}
