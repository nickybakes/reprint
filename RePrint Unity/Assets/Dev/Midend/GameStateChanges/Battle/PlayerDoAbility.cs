using System.Collections.Generic;
using UnityEngine;


public class PlayerDoAbility : BattleStateChange
{

    public new BattleStateChangeType Type
    {
        get { return BattleStateChangeType.PlayerDoAbility; }
    }

    private CharacterStats playerStats;

    private List<CharacterStats> enemyStats;

    private AbilitySelection abilitySelection;

    public PlayerDoAbility(AbilitySelection _abilitySelection, CharacterStats _playerStats, List<CharacterStats> _enemyStats)
    {
        abilitySelection = _abilitySelection;
        playerStats = _playerStats;
        enemyStats = _enemyStats;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {

    }
}
