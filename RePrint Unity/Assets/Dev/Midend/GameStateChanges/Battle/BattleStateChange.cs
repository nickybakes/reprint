using UnityEngine;

public enum BattleStateChangeType
{
    Undefined = -1,
    CharacterAttack,
    PlayerTurnStart,
    BattleInitialized,
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

    public virtual void ParseChange(BattleView battleView, BattleController controller)
    {

    }
}
