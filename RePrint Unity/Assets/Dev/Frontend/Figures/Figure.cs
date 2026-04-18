using UnityEngine;

public class Figure : MonoBehaviour
{
    [SerializeField] protected MeshRenderer meshRendererCenter;

    [SerializeField] protected GameObject meshCenterOverride;

    public Vector3 Center
    {
        get
        {
            if (meshCenterOverride)
                return meshCenterOverride.transform.position;
            else if (meshRendererCenter)
                return meshRendererCenter.bounds.center;
            else
                return transform.position;
        }
    }
}
