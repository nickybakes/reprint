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

    public void AddAmount(StatChange stat, int amount)
    {
        switch (stat)
        {
            case StatChange.PhysicalDamageTaken:
                PhysicalDamageTaken += amount;
                break;
            case StatChange.HealthGained:
                HealthGained += amount;
                break;
            case StatChange.DodgeTaken:
                DodgeTaken += amount;
                break;
            case StatChange.DodgeGained:
                DodgeGained += amount;
                break;
            case StatChange.TurnPriorityGained:
                TurnPriorityGained += amount;
                break;
            case StatChange.ChainGained:
                ChainGained += amount;
                break;
            case StatChange.ChainSpent:
                ChainSpent += amount;
                break;
            case StatChange.ChainTaken:
                ChainTaken += amount;
                break;
        }
    }

    public int GetAmount(StatChange stat)
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
}