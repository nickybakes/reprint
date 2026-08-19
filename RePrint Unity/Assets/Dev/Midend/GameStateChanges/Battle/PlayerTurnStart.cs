using UnityEngine;


public class PlayerTurnStart : BattleStateChange
{

    private BattleManager battleManager;
    private StatChangeBreakdown statChangeBreakdown;

    private int turnIndex;

    public PlayerTurnStart(BattleManager _battleManager, int _turnIndex, StatChangeBreakdown _statChangeBreakdown)
    {
        battleManager = _battleManager;
        statChangeBreakdown = _statChangeBreakdown;
        turnIndex = _turnIndex;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        Debug.Log("Player turn start!");
        view.TurnDisplay.SetText((turnIndex + 1).ToString());
        view.BattleStatsPanel.DisableAllTargetSelection();
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(battleManager.Player.Stats);
        view.BattleStatsPanel.UpdateAllEnemyStats(battleManager.EnemyTeam);
        view.BattleStatsPanel.PlayerStatPanel.Show();
        view.PlayerConfirmSequenceButton.Show();
        view.PlayerAbilitySequenceGroup.Clear();
        view.PlayerAbilityDisplayGroup.ResetSequenceState(battleManager.Player.Stats);
        view.PlayerAbilityDisplayGroup.Show();
        view.StatPopupGroupMods.DisplayModResults(statChangeBreakdown.modResults, battleManager.Player);
        view.EnablePlayerInteractions();
    }
}
