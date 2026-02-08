using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RePrintButton : RePrintSelectable, IPointerDownHandler, IPointerUpHandler, ISubmitHandler
{
    [SerializeField] protected UnityEvent submitEvent;

    [SerializeField] private float cooldownTime = .1f;

    private float timeSinceSubmit;

    private bool pressed;

    public virtual void Submit()
    {
        if (!IsActive() || !IsInteractable() || timeSinceSubmit < cooldownTime)
            return;

        timeSinceSubmit = 0;

        animator.SetTrigger("Press");
        animator.SetTrigger("Release");

        pressed = false;

        submitEvent.Invoke();
    }

    public virtual void Press()
    {
        animator.ResetTrigger("Release");
        animator.SetTrigger("Press");
        pressed = true;
    }

    public virtual void Release()
    {
        animator.SetTrigger("Release");
        pressed = false;
        if (isPointerInside)
        {
            Submit();
        }
    }

    public override void OnSelected()
    {
        if (!hasSelection)
        {
            if (pressed)
            {
                animator.SetTrigger("Press");
            }
            else
            {
                animator.SetTrigger("Select");
                selectEvent.Invoke(index);
            }
            hasSelection = true;
        }
    }

    void Update()
    {
        timeSinceSubmit += Time.deltaTime;
    }

    /// <summary>
    /// Evaluate current state and transition to pressed state.
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Selection tracking
        if (IsInteractable() && EventSystem.current != null)
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

        Release();
    }

    /// <summary>
    /// Call all registered ISubmitHandler.
    /// </summary>
    public virtual void OnSubmit(BaseEventData eventData)
    {
        Submit();
    }
}
