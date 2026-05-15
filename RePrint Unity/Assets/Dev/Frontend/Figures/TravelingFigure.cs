using System;
using UnityEngine;

public class TravelingFigure : Figure
{
    /// <summary>
    /// The transform data to travel from.
    /// </summary>
    private TransformDetails startTransformData;

    /// <summary>
    /// The transform data to travel towards.
    /// </summary>
    private TransformDetails goalTransformData;

    /// <summary>
    /// An animation curve that defines how the travel goes.
    /// </summary>
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    /// How long a travel takes, in seconds.
    /// </summary>
    [SerializeField] private float travelTimeLength = .2f;

    /// <summary>
    /// Current time, in seconds, that the figure has been traveling.
    /// </summary>
    private float currentTravelTime;

    /// <summary>
    /// Callback to call when the figure reaches its destination and finished traveling.
    /// </summary>
    private Action<TravelingFigure> arrivalCallback = null;

    /// <summary>
    /// Sets up the travel data.
    /// </summary>
    void Awake()
    {
        SetupTravelingTransformData();
    }

    /// <summary>
    /// Sets up the travel data.
    /// </summary>
    protected void SetupTravelingTransformData()
    {
        startTransformData = new TransformDetails(transform);
        goalTransformData = new TransformDetails(transform);
    }

    public Vector3 GetStartPosition()
    {
        return startTransformData.position;
    }

    public Vector3 GetGoalPosition()
    {
        return goalTransformData.position;
    }

    /// <summary>
    /// Set the current traveling time to 0.
    /// </summary>
    public void StartTraveling()
    {
        currentTravelTime = 0;
    }

    /// <summary>
    /// Fully stop traveling.
    /// </summary>
    public void StopTraveling()
    {
        currentTravelTime = travelTimeLength + 1;
    }

    /// <summary>
    /// Start traveling to a given transform.
    /// </summary>
    /// <param name="_transform">The goal transform.</param>
    /// <param name="_arrivalCallback">Optional callback to be called one the goal is reached.</param>
    public void TravelToTransform(Transform _transform, Action<TravelingFigure> _arrivalCallback = null)
    {
        SetStartTransform(transform);
        SetGoalTransform(_transform);
        StartTraveling();
        arrivalCallback = _arrivalCallback;
    }

    /// <summary>
    /// Start traveling to a given position, rotation, and scale.
    /// </summary>
    /// <param name="position">The goal position.</param>
    /// <param name="rotation">The goal rotation.</param>
    /// <param name="scale">The goal scale.</param>
    /// <param name="_arrivalCallback">Optional callback to be called one the goal is reached.</param>
    public void TravelToTransform(Vector3 position, Quaternion rotation, Vector3 scale, Action<TravelingFigure> _arrivalCallback = null)
    {
        SetStartTransform(transform);
        SetGoalTransform(position, rotation, scale);
        StartTraveling();
        arrivalCallback = _arrivalCallback;
    }

    /// <summary>
    /// Set start transform to a transform.
    /// </summary>
    /// <param name="_transform">The transform to use.</param>
    public void SetStartTransform(Transform _transform)
    {
        startTransformData.SetTransformData(_transform);
    }

    /// <summary>
    /// Set goal transform to a transform.
    /// </summary>
    /// <param name="_transform">The transform to use.</param>
    public void SetGoalTransform(Transform _transform)
    {
        goalTransformData.SetTransformData(_transform);
    }

    /// <summary>
    /// Set start transform to a given position, rotation, and scale.
    /// </summary>
    /// <param name="position">The start position.</param>
    /// <param name="rotation">The start rotation.</param>
    /// <param name="scale">The start scale.</param>
    public void SetStartTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        startTransformData.SetTransformData(position, rotation, scale);
    }

    /// <summary>
    /// Set goal transform to a given position, rotation, and scale.
    /// </summary>
    /// <param name="position">The goal position.</param>
    /// <param name="rotation">The goal rotation.</param>
    /// <param name="scale">The goal scale.</param>
    public void SetGoalTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        goalTransformData.SetTransformData(position, rotation, scale);
    }

    /// <summary>
    /// Apply a parent's transformations to the start transform.
    /// </summary>
    /// <param name="parent">The transform to use as a parent.</param>
    /// <param name="applyRotation">Whether to apply the parent's rotation.</param>
    /// <param name="applyScale">Whether to apply the parent's scale.</param>
    public void ApplyTransformParentToStartTransform(Transform parent, bool applyRotation = true, bool applyScale = true)
    {
        startTransformData.ApplyTransformParentToData(parent, applyRotation, applyScale);
    }


    /// <summary>
    /// Apply a parent's transformations to the goal transform.
    /// </summary>
    /// <param name="parent">The transform to use as a parent.</param>
    /// <param name="applyRotation">Whether to apply the parent's rotation.</param>
    /// <param name="applyScale">Whether to apply the parent's scale.</param>
    public void ApplyTransformParentToGoalTransform(Transform parent, bool applyRotation = true, bool applyScale = true)
    {
        goalTransformData.ApplyTransformParentToData(parent, applyRotation, applyScale);
    }

    /// <summary>
    /// Lerp between the start and goal transforms and apply it to the figure.
    /// </summary>
    /// <param name="t">The lerp value.</param>
    public void ApplyLerpTransform(float t)
    {
        transform.position = Vector3.LerpUnclamped(startTransformData.position, goalTransformData.position, t);
        transform.rotation = Quaternion.LerpUnclamped(startTransformData.rotation, goalTransformData.rotation, t);
        transform.localScale = Vector3.LerpUnclamped(startTransformData.scale, goalTransformData.scale, t);
    }

    /// <summary>
    /// Apply the start transform to the figure.
    /// </summary>
    public void ApplyStartTransform()
    {
        startTransformData.ApplyDataToTranform(transform);
    }

    /// <summary>
    /// Apply the goal transform to the figure.
    /// </summary>
    public void ApplyGoalTransform()
    {
        goalTransformData.ApplyDataToTranform(transform);
    }

    /// <summary>
    /// Whether this is currently traveling.
    /// </summary>
    /// <returns>Whether this is currently traveling.</returns>
    public bool isTraveling()
    {
        return currentTravelTime <= travelTimeLength;
    }

    /// <summary>
    /// Updates progress of traveling. If done traveling, applies the goal position to the figure.
    /// </summary>
    public void UpdateTravel()
    {
        if (isTraveling())
        {
            currentTravelTime += Time.deltaTime;
            float lerpT = currentTravelTime / travelTimeLength;
            lerpT = moveCurve.Evaluate(lerpT);
            ApplyLerpTransform(lerpT);
            if (!isTraveling() && arrivalCallback != null)
            {
                arrivalCallback.Invoke(this);
                arrivalCallback = null;
            }
        }
        else
        {
            ApplyGoalTransform();
        }
    }
}
