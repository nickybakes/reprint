using UnityEngine;


public class BattleInitialized : BattleStateChange
{

    public new BattleStateChangeType Type
    {
        get { return BattleStateChangeType.BattleInitialized; }
    }

    private Character playerCharacter;
    private Team enemyTeam;

    public BattleInitialized(Character _playerCharacter, Team _enemyTeam)
    {
        playerCharacter = _playerCharacter;
        enemyTeam = _enemyTeam;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        // Tell the View to spawn in the character visuals/UI elements
        view.PlayerAbilityDisplayGroup.AddAbilities(playerCharacter.Abilities, controller);
        view.PlayerFigureGroup.AddCharacter(playerCharacter, controller);
        view.EnemyFigureGroup.AddCharacters(enemyTeam.Members, controller);
        view.BattleStatsPanel.AddPlayerStatsPanel(playerCharacter);
        view.BattleStatsPanel.AddEnemyStatsPanels(enemyTeam);
        view.BattleStatsPanel.DisableAllTargetSelection();
        Debug.Log("Battle Initialized.");
    }
}
