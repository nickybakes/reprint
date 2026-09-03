using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [field: SerializeField] public CharacterProfile Profile { get; private set; }

    public ValueInput maxHealth;


    public PlayerAbilityData[] abilities;

    [Header("Player Specific")]
    public ValueInput abilityPointsMax;
    public ValueInput critDamageMultiplier;
    public ValueInput baseCritChance;

}
