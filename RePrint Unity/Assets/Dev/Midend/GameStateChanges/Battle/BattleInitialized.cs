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

    public override void ParseChange(BattleView battleView, BattleController controller)
    {
        // Tell the View to spawn in the character visuals/UI elements
        battleView.PlayerAbilityDisplayGroup.AddAbilities(playerCharacter.Abilities, controller);
        controller.AddTargetDisplays(enemyTeam);
        Debug.Log("Battle Initialized.");
    }
}
