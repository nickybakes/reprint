using System;
using UnityEngine;

[Serializable]
public class CameraOffset
{
    [SerializeField] private Vector3 positionOffset;

    [SerializeField] private float fovOffset;

    public Vector3 PositionOffset { get => positionOffset; }

    public float FOVOffset { get => fovOffset; }


}
