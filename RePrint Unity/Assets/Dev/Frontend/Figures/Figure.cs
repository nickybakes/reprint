using UnityEngine;

public class Figure : MonoBehaviour
{
    [SerializeField] protected MeshRenderer meshRenderer;

    [SerializeField] protected GameObject meshCenterOverride;

    public Vector3 Center
    {
        get
        {
            if (meshCenterOverride)
                return meshCenterOverride.transform.position;
            else
                return meshRenderer.bounds.center;
        }
    }
}
