

public enum GameValueType
{
    Chain,
    NumberOfEnemies,
    TotalOverclock
}


public class GameValues
{
    public GameEvent gameEvent;
    public int abilitySeqIndex;
    public int abilitySeqLength;
    public AbilitySequence abilitySequence;
    public Ability ability;
    public Character activator;
    public Character target;
    public BattleManager battleManager;
    public StatChangeBreakdown currentStatChangeBreakdown;

    public int GetIntGameValue(GameValueType valueType)
    {
        switch (valueType)
        {
            case GameValueType.Chain:
                return activator.Stats.Chain + activator.Stats.TempChain;
            case GameValueType.TotalOverclock:
                return abilitySequence.GetTotalOverclock();
        }

        return 0;
    }
}
