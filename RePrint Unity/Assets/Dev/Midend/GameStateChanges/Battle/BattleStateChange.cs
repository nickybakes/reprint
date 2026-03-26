using UnityEngine;

public enum BattleStateChangeType
{
    Undefined = -1,
    PlayerDoAbility,
    PlayerTurnStart,
    BattleInitialized,
    PlayerChangeAbilitySequence,
    PlayerTurnEnd,
}

public class BattleStateChange : GameStateChange
{

    public virtual BattleStateChangeType Type
    {
        get { return BattleStateChangeType.Undefined; }
    }

    public BattleStateChange()
    {

    }

    public virtual void ParseChange(BattleView view, BattleController controller)
    {

    }
}
