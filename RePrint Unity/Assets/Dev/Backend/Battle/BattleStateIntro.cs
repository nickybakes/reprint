using UnityEngine;

public class BattleStateIntro : BattleState
{
    public override void StartState()
    {
        BattleManager.battle.ui.CloseActionSequencePanel();
        BattleManager.battle.ui.ClosePlayerActionMenu();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (timeInState > .1)
        {
            BattleManager.battle.SwitchBattleState(BattleStateIndex.PlayerCreateActionSequence);
        }
    }
}
