using System.Collections.Generic;

public record CharacterStats
{
    public int Health { get; set; }
    public int HealthMax { get; set; }

    public int Chain { get; set; }

    public int AbilityPoints { get; set; }
    public int AbilityPointsMax { get; set; }

    public CharacterStats()
    {

    }

    public CharacterStats(CharacterStats copySource)
    {
        CopyFrom(copySource);
    }

    public void CopyFrom(CharacterStats source)
    {
        Health = source.Health;
        HealthMax = source.HealthMax;
        Chain = source.Chain;
        AbilityPoints = source.AbilityPoints;
        AbilityPointsMax = source.AbilityPointsMax;
    }
}