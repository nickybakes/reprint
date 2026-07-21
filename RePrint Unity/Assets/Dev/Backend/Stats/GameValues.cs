

public enum GameValueType
{
    Chain,
    NumberOfEnemies
}


public class GameValues
{
    public GameEvent gameEvent;
    public int abilitySeqIndex;
    public int abilitySeqLength;
    public Ability ability;
    public Character activator;
    public Character target;
    public BattleManager battleManager;
    public StatChangeBreakdown currentStatChangeBreakdown;
}
