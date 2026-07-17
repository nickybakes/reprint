using UnityEngine;

[CreateAssetMenu(fileName = "ModProfile", menuName = "Scriptable Objects/Mod Profile")]
public class ModProfile : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public AnimationTrigger Animation { get; private set; }

}
