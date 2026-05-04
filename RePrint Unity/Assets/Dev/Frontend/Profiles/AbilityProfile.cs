using UnityEngine;

[CreateAssetMenu(fileName = "AbilityProfile", menuName = "Scriptable Objects/AbilityProfile")]
public class AbilityProfile : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public AnimationTrigger Animation { get; private set; }

}
