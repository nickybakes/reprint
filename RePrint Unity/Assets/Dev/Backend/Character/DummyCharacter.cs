using System.Collections.Generic;

public class DummyCharacter : Character
{
    public DummyCharacter(BattleManager _battleManager) : base()
    {
        battleManager = _battleManager;
    }

    public override void ResetForTurn()
    {
        UniqueCharactersHitThisTurn = new List<Character>();
        CurrentHitsInAbility = 0;
        CurrentHitsInTurn = 0;
    }
}