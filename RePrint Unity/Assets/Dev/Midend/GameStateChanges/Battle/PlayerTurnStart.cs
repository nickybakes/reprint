using UnityEngine;


public class PlayerTurnStart : BattleStateChange
{

    private CharacterStats playerAbilitySequencingStats;

    private int turnIndex;

    public PlayerTurnStart(CharacterStats _playerAbilitySequencingStats, int _turnIndex)
    {
        playerAbilitySequencingStats = _playerAbilitySequencingStats;
        turnIndex = _turnIndex;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        Debug.Log("Player turn start!");
        view.TurnDisplay.SetText((turnIndex + 1).ToString());
        view.BattleStatsPanel.DisableAllTargetSelection();
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(playerAbilitySequencingStats);
        view.PlayerConfirmSequenceButton.Show();
        view.PlayerAbilitySequenceGroup.Clear();
        view.PlayerAbilityDisplayGroup.ResetSequenceState(playerAbilitySequencingStats);
        view.PlayerAbilityDisplayGroup.Show();
        view.EnablePlayerInteractions();
    }
}
