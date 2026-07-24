using UnityEngine;


public class PlayerChangeAbilitySequence : BattleStateChange
{

    private AbilitySequence abilitySequence;

    private BattleManager battleManager;

    private AbilitySequenceChangeType changeType;


    public PlayerChangeAbilitySequence(AbilitySequence _abilitySequence, BattleManager _battleManager, AbilitySequenceChangeType _changeType)
    {
        abilitySequence = _abilitySequence;
        battleManager = _battleManager;
        changeType = _changeType;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        view.PlayerAbilitySequenceGroup.Refresh(abilitySequence);
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(battleManager.Player.Stats);
        view.BattleStatsPanel.UpdateAllEnemyStats(battleManager.EnemyTeam);
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
        view.PlayerAbilityDisplayGroup.RefreshSequenceState(abilitySequence, battleManager.Player.Stats, changeType);
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
