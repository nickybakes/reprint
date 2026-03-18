using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public CharacterVisualData visualData;

    public ValueInput maxHealth;

    public CharacterActionData[] actionDatas;

    [Header("Player Specific")]
    public ValueInput actionPointsMax;

}
