using System.Collections.Generic;
using UnityEngine;


public class EnemyDoAbility : BattleStateChange
{

    public StatChangeBreakdown statChangeBreakdown;

    public EnemyAbility enemyAbility;

    public EnemyDoAbility(EnemyAbility _enemyAbility, StatChangeBreakdown _results)
    {
        enemyAbility = _enemyAbility;
        statChangeBreakdown = _results;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(statChangeBreakdown.statsAfter.PlayerStats);
        view.BattleStatsPanel.UpdateAllEnemyStats(statChangeBreakdown.statsAfter.EnemyStats);
    }
}
