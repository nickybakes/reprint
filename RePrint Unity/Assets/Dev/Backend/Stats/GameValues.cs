

public enum GameValueType
{
    Chain,
    NumberOfEnemies,
    TotalOverclock,
    PreviousTurnTotalHits,
    PreviousTurnUniqueEnemiesHit,
    PreviousTurnTotalCombo,
}


public class GameValues
{
    public GameEvent gameEvent;
    public int abilitySeqIndex;
    public int abilitySeqLength;
    public AbilitySequence abilitySequence;
    public AbilitySelection currentAbilitySelection;
    public Ability ability;
    public AbilityType abilityType;
    public Character activator;
    public Character target;
    public BattleManager battleManager;
    public StatChangeBreakdown currentStatChangeBreakdown;
    public Mod currentMod;

    public int GetIntGameValue(GameValueType valueType)
    {
        switch (valueType)
        {
            case GameValueType.Chain:
                return activator.Stats.Chain + activator.Stats.TempChain;
            case GameValueType.TotalOverclock:
                return abilitySequence.GetTotalOverclock();
            case GameValueType.PreviousTurnTotalHits:
                if (battleManager.TurnIndex > 0)
                    return battleManager.turnHistory[battleManager.TurnIndex - 1].totalHits;
                break;
            case GameValueType.PreviousTurnUniqueEnemiesHit:
                if (battleManager.TurnIndex > 0)
                    return battleManager.turnHistory[battleManager.TurnIndex - 1].uniqueCharactersHit.Count;
                break;
            case GameValueType.PreviousTurnTotalCombo:
                if (battleManager.TurnIndex > 0)
                    return battleManager.turnHistory[battleManager.TurnIndex - 1].totalCombo;
                break;
        }

        return 0;
    }
}
