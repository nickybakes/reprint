using UnityEngine;


public class PlayerChangeAbilitySequence : BattleStateChange
{

    private AbilitySequence abilitySequence;

    private CharacterStats playerAbilitySequencingStats;

    private AbilitySequenceChangeType changeType;


    public PlayerChangeAbilitySequence(AbilitySequence _abilitySequence, CharacterStats _playerAbilitySequencingStats, AbilitySequenceChangeType _changeType)
    {
        abilitySequence = _abilitySequence;
        playerAbilitySequencingStats = _playerAbilitySequencingStats;
        changeType = _changeType;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        view.PlayerAbilitySequenceGroup.Refresh(abilitySequence);
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(playerAbilitySequencingStats);
        if (abilitySequence.Sequence.Count > 0 && !abilitySequence.GetLastSelection().TargetIsSet)
        {
            AbilitySelection selection = abilitySequence.GetLastSelection();
            if (selection.Ability.CanTargetEnemies(selection.Overclock))
            {
                view.BattleStatsPanel.EnableEnemySelection();
            }

            if (selection.Ability.CanTargetPlayer(selection.Overclock))
            {
                view.BattleStatsPanel.EnablePlayerSelection();
            }
        }
        else
        {
            view.BattleStatsPanel.DisableAllTargetSelection();
        }
        view.PlayerAbilityDisplayGroup.RefreshSequenceState(abilitySequence, playerAbilitySequencingStats, changeType);
    }
}

public enum AbilitySequenceChangeType
{
    Reset,
    SubmitAbility,
    UnsubmitAbility,
    IncreaseOverclock,
    DecreaseOverclock,
    SubmitTarget,
    UnsubmitTarget,
    None
}
