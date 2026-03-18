using UnityEngine;


public class CharacterAttack : BattleStateChange
{

    public new BattleStateChangeType Type
    {
        get { return BattleStateChangeType.CharacterAttack; }
    }

    public CharacterAttack()
    {

    }

    public override void ParseChange(BattleView battleView, BattleController controller)
    {

    }
}
