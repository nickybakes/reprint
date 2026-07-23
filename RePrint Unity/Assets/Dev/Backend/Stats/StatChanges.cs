public class StatChanges
{
    public int StarterPhysicalDamageTaken { get; set; }
    public int FinisherPhysicalDamageTaken { get; set; }
    public float StarterPhysicalDamageMultiplier { get; set; } = 1;
    public float FinisherPhysicalDamageMultiplier { get; set; } = 1;
    public int HealthGained { get; set; }
    public int DodgeTaken { get; set; }
    public int DodgeGained { get; set; }
    public int TurnPriorityGained { get; set; }
    public int ChainGained { get; set; }
    public int ChainSpent { get; set; }
    public int ChainTaken { get; set; }
    public int TempChainGained { get; set; }
    public int APMaxIncrease { get; set; }
    public int APMaxDecrease { get; set; }

    public void AddAmount(StatChange stat, float amount)
    {
        switch (stat)
        {
            case StatChange.StarterPhysicalDamageTaken:
                StarterPhysicalDamageTaken += (int)amount;
                break;
            case StatChange.StarterPhysicalDamageMultiplier:
                StarterPhysicalDamageMultiplier += amount;
                break;
            case StatChange.FinisherPhysicalDamageTaken:
                FinisherPhysicalDamageTaken += (int)amount;
                break;
            case StatChange.FinisherPhysicalDamageMultiplier:
                FinisherPhysicalDamageMultiplier += amount;
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
            case StatChange.TempChainGained:
                TempChainGained += (int)amount;
                break;
            case StatChange.APMaxIncrease:
                APMaxIncrease += (int)amount;
                break;
            case StatChange.APMaxDecrease:
                APMaxIncrease += (int)amount;
                break;
        }
    }

    public float GetAmount(StatChange stat)
    {
        switch (stat)
        {
            case StatChange.StarterPhysicalDamageTaken:
                return StarterPhysicalDamageTaken;
            case StatChange.StarterPhysicalDamageMultiplier:
                return StarterPhysicalDamageMultiplier;
            case StatChange.FinisherPhysicalDamageTaken:
                return FinisherPhysicalDamageTaken;
            case StatChange.FinisherPhysicalDamageMultiplier:
                return FinisherPhysicalDamageMultiplier;
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
            case StatChange.TempChainGained:
                return TempChainGained;
            case StatChange.APMaxIncrease:
                return APMaxIncrease;
            case StatChange.APMaxDecrease:
                return APMaxDecrease;
        }

        return 0;
    }
}

public enum StatChange
{
    StarterPhysicalDamageTaken,
    FinisherPhysicalDamageTaken,
    StarterPhysicalDamageMultiplier,
    FinisherPhysicalDamageMultiplier,
    HealthGained,
    DodgeTaken,
    DodgeGained,
    TurnPriorityGained,
    ChainGained,
    ChainSpent,
    ChainTaken,
    TempChainGained,
    APMaxIncrease,
    APMaxDecrease,
}