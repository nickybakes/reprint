using UnityEngine;


public class BattleInitialized : BattleStateChange
{

    private PlayerCharacter playerCharacter;
    private Team enemyTeam;

    public BattleInitialized(PlayerCharacter _playerCharacter, Team _enemyTeam)
    {
        playerCharacter = _playerCharacter;
        enemyTeam = _enemyTeam;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        // Tell the View to spawn in the character visuals/UI elements
        view.PlayerAbilityDisplayGroup.AddAbilities(playerCharacter.Abilities, controller);
        view.PlayerFigureGroup.AddCharacter(playerCharacter, view, controller);
        view.EnemyFigureGroup.AddCharacters(enemyTeam.Members, view, controller);
        view.BattleStatsPanel.AddPlayerStatsPanel(playerCharacter);
        view.BattleStatsPanel.AddEnemyStatsPanels(enemyTeam);
        view.BattleStatsPanel.DisableAllTargetSelection();
        Debug.Log("Battle Initialized.");
    }
}
