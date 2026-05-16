using UnityEngine;

[CreateAssetMenu(fileName = "ShakeProfile", menuName = "Scriptable Objects/ShakeProfile")]
public class ShakeProfile : ScriptableObject
{

    [field: SerializeField] public float Range { get; private set; }
    [field: SerializeField] public float Damping { get; private set; }

    [field: SerializeField] public float MaxAmount { get; private set; }

    [field: SerializeField] public float Speed { get; private set; }

    [field: SerializeField] public float Deviation { get; private set; }


}
