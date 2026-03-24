using UnityEngine;

/// <summary>
/// Stores the maleable data for a tranform without needing a transform component.
/// </summary>
public class TransformDetails
{
    /// <summary>
    /// The local anchored position.
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// The local rotation.
    /// </summary>
    public Quaternion rotation;

    /// <summary>
    /// The local scale.
    /// </summary>
    public Vector3 scale;

    /// <summary>
    /// The transform of the game object to owns this data.
    /// </summary>
    private Transform owner;


    /// <summary>
    /// Constructor initializes the position, rotation, and scale default values.
    /// </summary>
    /// <param name="_owner">The transform of the game object to owns this data.</param>
    public TransformDetails(Transform _owner)
    {
        position = new Vector3();
        rotation = Quaternion.identity;
        scale = new Vector3();
        owner = _owner;
    }

    /// <summary>
    /// Set the data of this to match a given transform.
    /// </summary>
    /// <param name="transform">The transform to match.</param>
    public void SetTransformData(Transform transform)
    {
        SetTransformData(transform.position, transform.rotation, transform.localScale);
    }

    /// <summary>
    /// Set the position, rotation, and scale.
    /// </summary>
    /// <param name="_position">The position.</param>
    /// <param name="_rotation">The rotation as a Quaternion.</param>
    /// <param name="_scale">The 3D scale.</param>
    public void SetTransformData(Vector3 _position, Quaternion _rotation, Vector3 _scale)
    {
        position = _position;
        rotation = _rotation;
        scale = _scale;
    }

    /// <summary>
    /// Applies the data stored in this to a transform.
    /// </summary>
    /// <param name="transform">The transform to apply onto.</param>
    public void ApplyDataToTranform(Transform transform)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }

    /// <summary>
    /// Applies a parent's transformations to this transform data.
    /// </summary>
    /// <param name="parent">The transform to use as a parent.</param>
    /// <param name="applyRotation">Whether to apply the parent's rotation.</param>
    /// <param name="applyScale">Whether to apply the parent's scale.</param>
    public void ApplyTransformParentToData(Transform parent, bool applyRotation = true, bool applyScale = true)
    {
        Transform currentParent = parent;
        while (currentParent != null && currentParent != owner.parent)
        {
            if (applyScale)
            {
                scale.x *= currentParent.localScale.x;
                scale.y *= currentParent.localScale.y;
                scale.z *= currentParent.localScale.z;
            }
            position = Vector3.Scale(position, currentParent.localScale);

            if (applyRotation)
            {
                rotation *= currentParent.localRotation;
            }
            position = currentParent.localRotation * position;

            position += currentParent.position;

            if (currentParent.parent is Transform)
                currentParent = currentParent.parent;
            else
                currentParent = null;
        }
    }
}
