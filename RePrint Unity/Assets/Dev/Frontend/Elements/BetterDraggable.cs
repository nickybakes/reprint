using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// An in-game button that is a bit more streamlined than the default Unity buttons.
/// </summary>
public class BetterDraggable : BetterSelectable, IPointerDownHandler, IPointerUpHandler, ISubmitHandler
{
    /// <summary>
    /// The event to call when the user starts dragging this element.
    /// </summary>
    [SerializeField] protected UnityEvent startDragEvent;

    public UnityEvent StartDragEvent { get => startDragEvent; }

    /// <summary>
    /// The event to call on each frame while dragging.
    /// </summary>
    [SerializeField] protected UnityEvent draggingUpdateEvent;

    /// <summary>
    /// The event to call when the user stops dragging this element.
    /// </summary>
    [SerializeField] protected UnityEvent stopDragEvent;
    public UnityEvent StopDragEvent { get => stopDragEvent; }


    [SerializeField] private float deadzone = 5f;

    /// <summary>
    /// Cooldown between when this button can be submitted again.
    /// </summary>
    [SerializeField] private float cooldownTime = .1f;
    [SerializeField] private float dragCooldownTime = .1f;

    private Vector2 startDragMousePosition;

    private bool isBeingDragged;

    /// <summary>
    /// The time since the last submission.
    /// </summary>
    private float timeSinceSubmit;

    private float timeSinceDragged;


    /// <summary>
    /// Whether the button is currently held down/pressed.
    /// </summary>
    private bool pressed;

    public virtual void StartDrag()
    {
        if (!IsActive() || !Interactable)
            return;

        timeSinceDragged = 0;

        SetAnimationTrigger("Release");

        isBeingDragged = true;
        startDragEvent.Invoke();
    }

    public virtual void StopDrag()
    {
        if (!IsActive() || !Interactable)
            return;

        Deselect();
        SetAnimationTrigger("Release");

        isBeingDragged = false;
        pressed = false;
        stopDragEvent.Invoke();
    }

    /// <summary>
    /// Play the press and release animation and invoke the submit event.
    /// </summary>
    public virtual void Submit()
    {
        if (!IsActive() || !Interactable || timeSinceSubmit < cooldownTime)
            return;

        timeSinceSubmit = 0;

        SetAnimationTrigger("Press");
        SetAnimationTrigger("Release");

        pressed = false;
    }

    /// <summary>
    /// When the user pressed down on the button.
    /// </summary>
    public virtual void Press()
    {
        if (!IsActive() || !Interactable)
            return;

        startDragMousePosition = UIView.view.MouseViewPosition;

        pressed = true;
    }

    /// <summary>
    /// When the user releases the button from being held down. Only submit if the cursor is inside the button.
    /// </summary>
    public virtual void Release()
    {
        if (!IsActive() || !Interactable)
            return;

        SetAnimationTrigger("Release");
        pressed = false;
        if (isPointerInside)
        {
            Submit();
        }
    }

    /// <summary>
    /// Add handling for click-holding then dragging off/on the button.
    /// </summary>
    public override void OnSelected()
    {
        if (!IsActive() || !Interactable)
            return;

        if (!hasSelection)
        {
            if (pressed)
            {
                SetAnimationTrigger("Press");
            }
            else
            {
                SetAnimationTrigger("Select");
                selectEvent.Invoke(index);
            }
            hasSelection = true;
            if (SFXManager.sfx && selectSound)
            {
                SFXManager.sfx.Play(selectSound);
            }
        }
    }

    void Update()
    {
        timeSinceSubmit += Time.deltaTime;
        timeSinceDragged += Time.deltaTime;

        if (pressed && !isBeingDragged)
        {
            Vector2 currentMousePosition = UIView.view.MouseViewPosition;
            if (Vector2.Distance(startDragMousePosition, currentMousePosition) > deadzone)
            {
                StartDrag();
            }
        }
    }

    /// <summary>
    /// Evaluate current state and transition to pressed state.
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Selection tracking
        if (Interactable && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(gameObject, eventData);

        Press();
    }

    /// <summary>
    /// Evaluate eventData and transition to appropriate state.
    /// </summary>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!isBeingDragged)
        {
            Release();
        }
        else
        {
            StopDrag();
        }
    }

    /// <summary>
    /// Call all registered ISubmitHandler.
    /// </summary>
    public virtual void OnSubmit(BaseEventData eventData)
    {
        Submit();
    }
}