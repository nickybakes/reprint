using UnityEngine;


public class PlayerChangeAbilitySequence : BattleStateChange
{

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
        view.PlayerAbilityDisplayGroup.RefreshSequenceState(abilitySequence, playerAbilitySequencingStats);
    }
}

public enum AbilitySequenceChangeType
{
    Reset,
    SelectAbility,
    UnselectAbility,
    IncreaseOverclock,
    DecreaseOverclock,
    SelectTarget,
    UnselectTarget
}
