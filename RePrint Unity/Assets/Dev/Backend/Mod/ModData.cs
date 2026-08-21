using UnityEngine;

[CreateAssetMenu(fileName = "ModData", menuName = "Scriptable Objects/ModData")]
public class ModData : ScriptableObject
{
    [field: SerializeField] public ModProfile Profile { get; private set; }

    [field: SerializeField, HideInInspector] public string Name { get; private set; }

    [field: SerializeField] public Rarity Rarity { get; private set; }
    [field: SerializeField] public int SortOrder { get; private set; }
    [field: SerializeField] public BetterEditorList<ModBehavior> ModBehaviors { get; private set; }

}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Ethereal,
    Mythic
}
