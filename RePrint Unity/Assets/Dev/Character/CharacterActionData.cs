using UnityEngine;

[CreateAssetMenu(fileName = "CharacterActionData", menuName = "Scriptable Objects/CharacterActionData")]
public class CharacterActionData : ScriptableObject
{
    public new string name;

    [TextArea]
    public string description;

    public int damage;

    public int actionPointCost;
}
