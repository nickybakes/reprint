using System.Collections.Generic;
using UnityEngine;


public class EnemyDecideAbility : BattleStateChange
{

    private EnemyAbility enemyAbility;

    private EnemyCharacter enemyCharacter;

    public EnemyDecideAbility(EnemyAbility _enemyAbility, EnemyCharacter _enemyCharacter)
    {
        enemyAbility = _enemyAbility;
        enemyCharacter = _enemyCharacter;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        view.BattleStatsPanel.UpdateEnemyIntent(enemyAbility, enemyCharacter);
    }
}
