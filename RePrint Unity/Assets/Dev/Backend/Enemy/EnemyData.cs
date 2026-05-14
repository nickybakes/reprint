using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public CharacterProfile Profile { get; private set; }

    public ValueInput maxHealth;

    [field: SerializeField] public BetterEditorList<EnemyBehavior> Behaviors { get; private set; }
}
