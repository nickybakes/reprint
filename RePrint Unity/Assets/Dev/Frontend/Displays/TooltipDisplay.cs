using UnityEngine;

public class TooltipDisplay : TravelingDisplay
{

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetGoalTransform(UIView.view.MouseViewPosition, Quaternion.identity, Vector3.one);
        UpdateTravel();
    }
}
