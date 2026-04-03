using System.Collections.Generic;
using UnityEngine;


public class EnemyDoAbility : BattleStateChange
{

    private AbilityResults results;

    private EnemyAbility enemyAbility;

    public EnemyDoAbility(EnemyAbility _enemyAbility, AbilityResults _results)
    {
        enemyAbility = _enemyAbility;
        results = _results;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(results.PlayerStatsAfter);
        view.BattleStatsPanel.UpdateAllEnemyStats(results.EnemyStatsAfter);
    }
}
