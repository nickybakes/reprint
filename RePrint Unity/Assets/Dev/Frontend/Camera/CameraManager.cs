using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public enum CameraFocalPoint
{
    Default,
    Enemies
}

public class CameraManager : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform cameraOffset;

    [SerializeField] private Shaker cameraShaker;

    [SerializeField] private float transitionTime;
    [SerializeField] private AnimationCurve transitionEase;


    [SerializeField] private CameraOffset enemyFocusOffset;

    private Vector3 currentPositionOffset;
    private float currentFOVOffset;

    private Vector3 startPositionOffset;
    private float startFOVOffset;

    private Vector3 goalPositionOffset;
    private float goalFOVOffset;

    private float currentTransitionTime;

    private CameraFocalPoint currentFocalPoint;

    private float defaultFOV;

    void Awake()
    {
        defaultFOV = camera.fieldOfView;
        currentPositionOffset = new Vector3();
    }

    public void Shake(float amount)
    {
        cameraShaker.Shake(amount);
    }

    public void FocusEnemies()
    {
        if (currentFocalPoint != CameraFocalPoint.Enemies)
        {
            StartTransition();
            currentFocalPoint = CameraFocalPoint.Enemies;
            goalPositionOffset = enemyFocusOffset.PositionOffset;
            goalFOVOffset = enemyFocusOffset.FOVOffset;
        }
    }

    public void FocusDefault()
    {
        if (currentFocalPoint != CameraFocalPoint.Default)
        {
            StartTransition();
            currentFocalPoint = CameraFocalPoint.Default;
            goalPositionOffset = Vector3.zero;
            goalFOVOffset = 0;
        }
    }

    private void StartTransition()
    {
        startPositionOffset = currentPositionOffset;
        startFOVOffset = currentFOVOffset;
        currentTransitionTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTransitionTime += Time.deltaTime;

        float transitionLerp = Mathf.Clamp(currentTransitionTime / transitionTime, 0, 1);
        transitionLerp = transitionEase.Evaluate(transitionLerp);

        currentPositionOffset = Vector3.Lerp(startPositionOffset, goalPositionOffset, transitionLerp);
        currentFOVOffset = Mathf.Lerp(startFOVOffset, goalFOVOffset, transitionLerp);

        cameraOffset.transform.localPosition = currentPositionOffset;
        camera.fieldOfView = defaultFOV + currentFOVOffset;
    }
}
