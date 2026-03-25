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
        Health = copySource.Health;
        HealthMax = copySource.HealthMax;
        Chain = copySource.Chain;
        AbilityPoints = copySource.AbilityPoints;
        AbilityPointsMax = copySource.AbilityPointsMax;
    }
}