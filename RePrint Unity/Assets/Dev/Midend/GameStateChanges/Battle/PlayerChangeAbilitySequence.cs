using UnityEngine;


public class PlayerChangeAbilitySequence : BattleStateChange
{

    public new BattleStateChangeType Type
    {
        get { return BattleStateChangeType.PlayerChangeAbilitySequence; }
    }

    private AbilitySequence abilitySequence;

    public PlayerChangeAbilitySequence(AbilitySequence _abilitySequence)
    {
        abilitySequence = _abilitySequence;
    }

    public override void ParseChange(BattleView battleView, BattleController controller)
    {
        battleView.PlayerAbilityDisplayGroup.RefreshSequenceState(abilitySequence);
    }
}
