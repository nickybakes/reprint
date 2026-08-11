

using UnityEngine;

[CreateAssetMenu(fileName = "RarityProfile", menuName = "Scriptable Objects/Rarity Profile")]
public class RarityProfile : ScriptableObject
{

    [field: SerializeField] public Color CommonColor { get; private set; } = Color.white;
    [field: SerializeField] public string CommonString { get; private set; }

    [field: SerializeField] public Color UncommonColor { get; private set; } = Color.white;
    [field: SerializeField] public string UncommonString { get; private set; }

    [field: SerializeField] public Color RareColor { get; private set; } = Color.white;
    [field: SerializeField] public string RareString { get; private set; }

    [field: SerializeField] public Color EtherealColor { get; private set; } = Color.white;
    [field: SerializeField] public string EtherealString { get; private set; }

    [field: SerializeField] public Color MythicColor { get; private set; } = Color.white;
    [field: SerializeField] public string MythicString { get; private set; }


}