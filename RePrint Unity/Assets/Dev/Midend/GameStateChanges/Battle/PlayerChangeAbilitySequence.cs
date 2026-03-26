using UnityEngine;


public class PlayerChangeAbilitySequence : BattleStateChange
{

    public new BattleStateChangeType Type
    {
        get { return BattleStateChangeType.PlayerChangeAbilitySequence; }
    }

    private AbilitySequence abilitySequence;

    private CharacterStats playerAbilitySequencingStats;


    public PlayerChangeAbilitySequence(AbilitySequence _abilitySequence, CharacterStats _playerAbilitySequencingStats)
    {
        abilitySequence = _abilitySequence;
        playerAbilitySequencingStats = _playerAbilitySequencingStats;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        view.PlayerAbilitySequenceGroup.Refresh(abilitySequence);
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(playerAbilitySequencingStats);
        if (abilitySequence.Sequence.Count > 0 && !abilitySequence.GetLastSelection().TargetIsSet)
        {
            view.BattleStatsPanel.EnableEnemySelection();
        }
        else
        {
            view.BattleStatsPanel.DisableAllTargetSelection();
        }
        view.PlayerAbilityDisplayGroup.RefreshSequenceState(abilitySequence, playerAbilitySequencingStats);
    }
}
