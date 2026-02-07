using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class RePrintSelectable :
        UIBehaviour,
        IMoveHandler,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler
{
    private bool m_EnableCalled = false;

    private bool m_GroupsAllowInteraction = true;
    protected int m_CurrentIndex = -1;

    [Tooltip("Can the Selectable be interacted with?")]
    [SerializeField]
    private bool m_Interactable = true;

    [Header("Navigation")]
    [SerializeField] private RePrintSelectable upSelection;
    [SerializeField] private RePrintSelectable downSelection;
    [SerializeField] private RePrintSelectable leftSelection;
    [SerializeField] private RePrintSelectable rightSelection;

    [SerializeField] private UnityEvent<int> selectEvent;
    [SerializeField] private UnityEvent<int> deselectEvent;


    protected Animator animator;


    /// <summary>
    /// Is this object interactable.
    /// </summary>
    /// <example>
    public bool interactable
    {
        get { return m_Interactable; }
        set
        {
            m_Interactable = value;
            if (!m_Interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }

    protected bool isPointerInside { get; set; }
    protected bool hasSelection { get; set; }

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Is the object interactable.
    /// </summary>
    public virtual bool IsInteractable()
    {
        return m_GroupsAllowInteraction && m_Interactable;
    }

    // Select on enable and add to the list.
    protected override void OnEnable()
    {
        //Check to avoid multiple OnEnable() calls for each selectable
        if (m_EnableCalled)
            return;

        base.OnEnable();

        if (EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            hasSelection = true;
        }

        m_EnableCalled = true;
    }

    // Remove from the list.
    protected override void OnDisable()
    {
        //Check to avoid multiple OnDisable() calls for each selectable
        if (!m_EnableCalled)
            return;

        InstantClearState();
        base.OnDisable();

        m_EnableCalled = false;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            InstantClearState();
        }
    }

    /// <summary>
    /// Clear any internal state from the Selectable (used when disabling).
    /// </summary>
    protected virtual void InstantClearState()
    {
        isPointerInside = false;
        hasSelection = false;
        OnReset();
    }

    public virtual void OnReset()
    {
        animator.SetTrigger("Reset");
    }

    public virtual void OnSelected()
    {
        if (!hasSelection)
        {
            animator.SetTrigger("Select");
            hasSelection = true;
        }
    }

    public virtual void OnDeselected()
    {
        if (hasSelection)
        {
            animator.SetTrigger("Deselect");
            hasSelection = false;
        }
    }

    /// <summary>
    /// Evaluate current state and transition to appropriate state.
    /// New state could be pressed or hover depending on pressed state.
    /// </summary>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        OnSelected();
    }

    /// <summary>
    /// Evaluate current state and transition to normal state.
    /// </summary>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        OnDeselected();
    }

    /// <summary>
    /// Set selection and transition to appropriate state.
    /// </summary>
    public virtual void OnSelect(BaseEventData eventData)
    {
        OnSelected();
    }

    /// <summary>
    /// Unset selection and transition to appropriate state.
    /// </summary>
    public virtual void OnDeselect(BaseEventData eventData)
    {
        OnDeselected();
    }

    /// <summary>
    /// Selects this Selectable.
    /// </summary>
    public virtual void Select()
    {
        if (EventSystem.current == null || EventSystem.current.alreadySelecting)
            return;

        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    /// <summary>
    /// Determine in which of the 4 move directions the next selectable object should be found.
    /// </summary>
    public virtual void OnMove(AxisEventData eventData)
    {
        switch (eventData.moveDir)
        {
            case MoveDirection.Right:
                // Navigate(eventData, FindSelectableOnRight());
                break;

            case MoveDirection.Up:
                // Navigate(eventData, FindSelectableOnUp());
                break;

            case MoveDirection.Left:
                // Navigate(eventData, FindSelectableOnLeft());
                break;

            case MoveDirection.Down:
                // Navigate(eventData, FindSelectableOnDown());
                break;
        }
    }
}
