

using UnityEngine;

public class LerpDetails
{
    private float currentLerp;
    private float start;
    private float goal;
    private float currentTransitionTime;
    private float transitionTime;

    public float CurrentLerp { get => currentLerp; }

    public LerpDetails()
    {
        currentLerp = 0;
        start = 0;
        goal = 0;
        currentTransitionTime = 1;
        transitionTime = 1;
    }

    public void TransitionTo(float newGoal, float time)
    {
        start = currentLerp;
        goal = newGoal;
        currentTransitionTime = 0;
        transitionTime = time;

        if (transitionTime == 0)
        {
            currentLerp = goal;
            currentTransitionTime = 1;
            transitionTime = 1;
        }
    }

    public void Update()
    {
        currentTransitionTime += Time.deltaTime;

        float transitionLerp = Mathf.Clamp(currentTransitionTime / transitionTime, 0, 1);

        currentLerp = Mathf.Lerp(start, goal, transitionLerp);
    }
}