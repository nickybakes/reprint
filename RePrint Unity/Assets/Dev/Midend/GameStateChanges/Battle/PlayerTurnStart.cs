using UnityEngine;


public class PlayerTurnStart : BattleStateChange
{

    public new BattleStateChangeType Type
    {
        get { return BattleStateChangeType.PlayerTurnStart; }
    }

    public PlayerTurnStart()
    {

    }

    public override void ParseChange(BattleView battleView, BattleController controller)
    {
        Debug.Log("Player turn start!");
        battleView.EnablePlayerInteractions();
    }
}
