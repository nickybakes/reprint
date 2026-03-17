using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Scriptable Objects/BattleData")]
public class BattleData : ScriptableObject
{

    public CharacterData playerCharacterData;

    public CharacterData[] enemyCharacterDatas;

}
