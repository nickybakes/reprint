public class StatChanges
{
    public int PhysicalDamageTaken { get; set; }
    public int HealthGained { get; set; }
    public int DodgeTaken { get; set; }
    public int DodgeGained { get; set; }
    public int TurnPriorityGained { get; set; }
    public int ChainGained { get; set; }
    public int ChainSpent { get; set; }
    public int ChainTaken { get; set; }
    public float StarterPhysicalDamageMultiplier { get; set; }

    public void AddAmount(StatChange stat, float amount)
    {
        switch (stat)
        {
            case StatChange.PhysicalDamageTaken:
                PhysicalDamageTaken += (int)amount;
                break;
            case StatChange.HealthGained:
                HealthGained += (int)amount;
                break;
            case StatChange.DodgeTaken:
                DodgeTaken += (int)amount;
                break;
            case StatChange.DodgeGained:
                DodgeGained += (int)amount;
                break;
            case StatChange.TurnPriorityGained:
                TurnPriorityGained += (int)amount;
                break;
            case StatChange.ChainGained:
                ChainGained += (int)amount;
                break;
            case StatChange.ChainSpent:
                ChainSpent += (int)amount;
                break;
            case StatChange.ChainTaken:
                ChainTaken += (int)amount;
                break;
            case StatChange.StarterPhysicalDamageMultiplier:
                StarterPhysicalDamageMultiplier += amount;
                break;
        }
    }

    public float GetAmount(StatChange stat)
    {
        switch (stat)
        {
            case StatChange.PhysicalDamageTaken:
                return PhysicalDamageTaken;
            case StatChange.HealthGained:
                return HealthGained;
            case StatChange.DodgeTaken:
                return DodgeTaken;
            case StatChange.DodgeGained:
                return DodgeGained;
            case StatChange.TurnPriorityGained:
                return TurnPriorityGained;
            case StatChange.ChainGained:
                return ChainGained;
            case StatChange.ChainSpent:
                return ChainSpent;
            case StatChange.ChainTaken:
                return ChainTaken;
            case StatChange.StarterPhysicalDamageMultiplier:
                return StarterPhysicalDamageMultiplier;
        }

        return 0;
    }
}

public enum StatChange
{
    PhysicalDamageTaken,
    HealthGained,
    DodgeTaken,
    DodgeGained,
    TurnPriorityGained,
    ChainGained,
    ChainSpent,
    ChainTaken,
    StarterPhysicalDamageMultiplier
}