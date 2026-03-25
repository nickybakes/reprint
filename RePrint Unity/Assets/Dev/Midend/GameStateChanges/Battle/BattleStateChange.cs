using UnityEngine;

public enum BattleStateChangeType
{
    Undefined = -1,
    CharacterAttack,
    PlayerTurnStart,
    BattleInitialized,
    PlayerChangeAbilitySequence,
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
