using UnityEngine;

public class TooltipDisplay : TravelingDisplay
{

    public void Show()
    {
        MoveToMousePosition();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        MoveToMousePosition();
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
        MoveToMousePosition();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToMousePosition();
    }

    protected void MoveToMousePosition()
    {
        SetGoalTransform(UIView.view.MouseViewPosition, Quaternion.identity, Vector3.one);
        UpdateTravel();
    }
}
