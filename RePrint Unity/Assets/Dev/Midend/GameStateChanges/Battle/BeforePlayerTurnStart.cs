using UnityEngine;


public class BeforePlayerTurnStart : BattleStateChange
{

    private BattleManager battleManager;

    public BeforePlayerTurnStart(BattleManager _battleManager)
    {
        battleManager = _battleManager;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(battleManager.Player.Stats);
        view.BattleStatsPanel.UpdateAllEnemyStats(battleManager.EnemyTeam);
    }
}
