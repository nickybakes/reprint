using UnityEngine;

public enum ShakeAxis
{
    XY,
    XZ,
    ZY
}

public class Shaker : MonoBehaviour
{
    [SerializeField] private ShakeProfile shakerProfile;
    [SerializeField] private ShakeAxis axis;

    private float currentAmount;
    private float currentSin;
    private float currentCos;

    private float sinOffset;
    private float cosOffset;

    private Vector3 newPosition;

    void Awake()
    {
        newPosition = new Vector3();
    }

    public void Shake(float amount)
    {
        currentAmount = Mathf.Min(shakerProfile.MaxAmount, currentAmount += shakerProfile.Range * amount);

        sinOffset = Random.value * shakerProfile.Deviation;
        cosOffset = Random.value * shakerProfile.Deviation;
    }

    void Update()
    {
        currentSin = Mathf.Sin(Time.time * shakerProfile.Speed + sinOffset);
        currentCos = Mathf.Sin(Time.time * shakerProfile.Speed + cosOffset);

        switch (axis)
        {
            case ShakeAxis.XY:
                newPosition.x = currentCos * currentAmount;
                newPosition.y = currentSin * currentAmount;
                break;

            case ShakeAxis.XZ:
                newPosition.x = currentCos * currentAmount;
                newPosition.z = currentSin * currentAmount;
                break;

            case ShakeAxis.ZY:
                newPosition.z = currentCos * currentAmount;
                newPosition.y = currentSin * currentAmount;
                break;
        }

        transform.localPosition = newPosition;

        currentAmount = Mathf.Max(0, currentAmount - shakerProfile.Damping * Time.deltaTime);
    }
}
