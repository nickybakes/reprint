using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Scriptable Objects/CharacterProfile")]
public class CharacterProfile : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField, TextArea] public string Description { get; private set; }

    [field: SerializeField] public CharacterFigure Figure { get; private set; }

}
