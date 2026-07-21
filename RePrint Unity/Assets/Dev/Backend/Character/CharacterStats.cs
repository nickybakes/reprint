using System.Collections.Generic;

public record CharacterStats
{
    public Character Character { get; set; }
    public int Health { get; set; }
    public int HealthMax { get; set; }

    public int Chain { get; set; }
    public int Dodge { get; set; }

    public int AbilityPoints { get; set; }
    public int AbilityPointsMax { get; set; }

    public int PhysicalDamageTaken { get; set; }
    public int DodgeTaken { get; set; }

    public int BleedDamageTaken { get; set; }
    public int CriticalDamageTaken { get; set; }

    public float PhysicalDamageResistance { get; set; }
    public int PhysicalDamageShield { get; set; }



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
        Dodge = source.Dodge;
        AbilityPoints = source.AbilityPoints;
        AbilityPointsMax = source.AbilityPointsMax;
        PhysicalDamageTaken = source.PhysicalDamageTaken;
        DodgeTaken = source.DodgeTaken;
        BleedDamageTaken = source.BleedDamageTaken;
        CriticalDamageTaken = source.CriticalDamageTaken;
        PhysicalDamageResistance = source.PhysicalDamageResistance;
        PhysicalDamageShield = source.PhysicalDamageShield;
    }
}

public enum CharacterStat
{
    Health,
    AbilityPoints,
    Chain,
    PhysicalDamageTaken,
    BleedDamageTaken,
    CriticalDamageTaken,
}