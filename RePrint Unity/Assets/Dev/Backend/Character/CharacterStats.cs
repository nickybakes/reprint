using System.Collections.Generic;

public record CharacterStats
{
    public Character Character { get; set; }
    public int Health { get; set; }
    public int HealthMax { get; set; }

    public int Chain { get; set; }
    public int TempChain { get; set; }
    public int Dodge { get; set; }

    public int AbilityPoints { get; set; }
    public int AbilityPointsMax { get; set; }

    public int PhysicalDamageTaken { get; set; }
    public int DodgeTaken { get; set; }

    public int BleedDamageTaken { get; set; }
    public int CriticalDamageTaken { get; set; }

    public float PhysicalDamageResistance { get; set; }
    public int PhysicalDamageShield { get; set; }
    public int TurnPriority { get; set; }


    public CharacterStats(Character character)
    {
        Character = character;
    }

    public CharacterStats(CharacterStats copySource)
    {
        CopyFrom(copySource);
    }

    public void CopyFrom(CharacterStats source)
    {
        Character = source.Character;
        Health = source.Health;
        HealthMax = source.HealthMax;
        Chain = source.Chain;
        TempChain = source.TempChain;
        Dodge = source.Dodge;
        AbilityPoints = source.AbilityPoints;
        AbilityPointsMax = source.AbilityPointsMax;
        PhysicalDamageTaken = source.PhysicalDamageTaken;
        DodgeTaken = source.DodgeTaken;
        BleedDamageTaken = source.BleedDamageTaken;
        CriticalDamageTaken = source.CriticalDamageTaken;
        PhysicalDamageResistance = source.PhysicalDamageResistance;
        PhysicalDamageShield = source.PhysicalDamageShield;
        TurnPriority = source.TurnPriority;
    }

    public int GetStat(CharacterStat stat)
    {
        switch (stat)
        {
            case CharacterStat.Health:
                return Health;
            case CharacterStat.Chain:
                return Chain;
            case CharacterStat.AbilityPoints:
                return AbilityPoints;
        }

        return 0;
    }

    public float GetPercentage(CharacterStat stat)
    {
        switch (stat)
        {
            case CharacterStat.Health:
                return (float)Health / HealthMax;
            case CharacterStat.AbilityPoints:
                return (float)AbilityPoints / AbilityPointsMax;
        }

        return 0;
    }
}

public enum CharacterStat
{
    Health,
    Chain,
    AbilityPoints,
}