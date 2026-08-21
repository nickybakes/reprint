

using UnityEngine;

[CreateAssetMenu(fileName = "RarityDirectory", menuName = "Scriptable Objects/Rarity Directory")]
public class RarityDirectory : ScriptableObject
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



    public Color GetColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return CommonColor;
            case Rarity.Uncommon:
                return UncommonColor;
            case Rarity.Rare:
                return RareColor;
            case Rarity.Ethereal:
                return EtherealColor;
        }

        return MythicColor;
    }

    public string GetString(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return CommonString;
            case Rarity.Uncommon:
                return UncommonString;
            case Rarity.Rare:
                return RareString;
            case Rarity.Ethereal:
                return EtherealString;
        }

        return MythicString;
    }
}