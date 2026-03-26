using UnityEngine;


public class PlayerTurnStart : BattleStateChange
{

    public new BattleStateChangeType Type
    {
        get { return BattleStateChangeType.PlayerTurnStart; }
    }

    private CharacterStats playerAbilitySequencingStats;

    public PlayerTurnStart(CharacterStats _playerAbilitySequencingStats)
    {
        playerAbilitySequencingStats = _playerAbilitySequencingStats;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        Debug.Log("Player turn start!");
        view.PlayerAbilityDisplayGroup.ResetSequenceState(playerAbilitySequencingStats);
        view.BattleStatsPanel.DisableAllTargetSelection();
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(playerAbilitySequencingStats);
        view.PlayerConfirmSequenceButton.Show();
        view.PlayerAbilitySequenceGroup.Clear();
        view.PlayerAbilityDisplayGroup.Show();
        view.EnablePlayerInteractions();
    }
}
