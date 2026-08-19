using UnityEngine;

public class FollowFigureDisplay : Display
{
    [SerializeField] private Vector2 offset;

    protected CharacterFigure figure;

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    public void SetFigure(CharacterFigure _figure)
    {
        figure = _figure;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (figure)
        {
            rectTransform.anchoredPosition = UIView.WorldToCanvasPoint(figure.Center);
            rectTransform.anchoredPosition += offset;
        }
    }
}
