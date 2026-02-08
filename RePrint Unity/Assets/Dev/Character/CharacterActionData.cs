using UnityEngine;

[CreateAssetMenu(fileName = "CharacterActionData", menuName = "Scriptable Objects/CharacterActionData")]
public class CharacterActionData : ScriptableObject
{
    public string actionName;

    [TextArea]
    public string description;

    public int damage;

    public int actionPointCost;
}
