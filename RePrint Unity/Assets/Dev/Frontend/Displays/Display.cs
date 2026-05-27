using UnityEngine;

/// <summary>
/// A basic in-game UI component that has reference to its Rect Transform
/// </summary>
public class Display : MonoBehaviour
{
    /// <summary>
    /// Reference to this object's Rect Transform.
    /// </summary>
    protected RectTransform rectTransform;

    /// <summary>
    /// Reference to this object's parent's Rect Transform.
    /// </summary>
    protected RectTransform parentRectTransform;


    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    /// <summary>
    /// Sets up rect transform data.
    /// </summary>
    protected void SetupRectTransform()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Get the Rect Transform.
    /// </summary>
    /// <returns>The Rect Transform of this Display.</returns>
    public RectTransform GetRect()
    {
        return rectTransform;
    }

    /// <summary>
    /// Get the parent's Rect Transform.
    /// </summary>
    /// <returns>The Rect Transform of this Display's parent.</returns>
    public RectTransform GetParentRect()
    {
        return parentRectTransform;
    }
}