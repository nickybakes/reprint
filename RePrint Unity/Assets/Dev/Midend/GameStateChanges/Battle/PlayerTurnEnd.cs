using UnityEngine;


public class PlayerTurnEnd : BattleStateChange
{

    public new BattleStateChangeType Type
    {
        get { return BattleStateChangeType.PlayerTurnEnd; }
    }

    private CharacterStats playerAbilitySequencingStats;

    public PlayerTurnEnd(CharacterStats _playerAbilitySequencingStats)
    {
        playerAbilitySequencingStats = _playerAbilitySequencingStats;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        Debug.Log("Player turn end!");
        view.DisablePlayerInteractions();
        view.BattleStatsPanel.DisableAllTargetSelection();
        view.PlayerConfirmSequenceButton.Hide();
        view.PlayerAbilityDisplayGroup.Hide();
    }
}
