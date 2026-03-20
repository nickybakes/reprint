using UnityEngine;

[CreateAssetMenu(fileName = "ModData", menuName = "Scriptable Objects/ModData")]
public class ModData : ScriptableObject
{
    [field: SerializeField] public BetterEditorList<ModBehavior> ModBehaviors { get; private set; }

}
