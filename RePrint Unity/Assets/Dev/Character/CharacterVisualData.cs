using UnityEngine;

[CreateAssetMenu(fileName = "CharacterVisualData", menuName = "Scriptable Objects/CharacterVisualData")]
public class CharacterVisualData : ScriptableObject
{
    public GameObject model;

    public Vector3 meshCenterOffset;
}
