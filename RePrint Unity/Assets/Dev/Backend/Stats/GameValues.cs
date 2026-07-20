

public enum GameValueType
{
    Chain,
    NumberOfEnemies
}


public class GameValues
{
    public int abilitySeqIndex;
    public int abilitySeqLength;
    public Ability ability;
    public Character activator;
    public Character target;
    public BattleManager battleManager;
    public AbilityAmounts physicalDamageAmounts;
    public AbilityAmounts dodgeAmounts;
    public AbilityAmounts chainGainAmounts;
    public AbilityAmounts chainSpentAmounts;
}
