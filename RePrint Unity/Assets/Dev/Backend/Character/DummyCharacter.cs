using System.Collections.Generic;

public class DummyCharacter : Character
{
    public DummyCharacter(BattleManager _battleManager) : base(_battleManager)
    {

    }

    public override void ResetForTurn()
    {
        UniqueCharactersHit = new List<Character>();
        CurrentHits = 0;
    }
}