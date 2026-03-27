using UnityEngine;


public class PlayerTurnEnd : BattleStateChange
{

    public PlayerTurnEnd()
    {
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
