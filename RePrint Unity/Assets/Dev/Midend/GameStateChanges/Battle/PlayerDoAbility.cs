using System.Collections.Generic;
using UnityEngine;


public class PlayerDoAbility : BattleStateChange
{

    private AbilityResults results;

    private AbilitySelection abilitySelection;

    public PlayerDoAbility(AbilitySelection _abilitySelection, AbilityResults _results)
    {
        abilitySelection = _abilitySelection;
        results = _results;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        view.BattleStatsPanel.PlayerStatPanel.UpdateStats(results.PlayerStatsAfter);
        view.BattleStatsPanel.UpdateAllEnemyStats(results.EnemyStatsAfter);
    }
}
