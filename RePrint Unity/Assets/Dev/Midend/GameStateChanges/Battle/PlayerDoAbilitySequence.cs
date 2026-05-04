using System.Collections.Generic;
using UnityEngine;


public class PlayerDoAbilitySequence : BattleStateChange
{

    private List<AbilityResults> results;

    private List<AbilitySelection> abilitySelections;

    private Character player;
    private CharacterFigure playerFigure;

    private int currentAbilityIndex;

    private BattleView battleView;

    public PlayerDoAbilitySequence(Character _player, List<AbilitySelection> _abilitySelections, List<AbilityResults> _results)
    {
        player = _player;
        abilitySelections = _abilitySelections;
        results = _results;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        battleView = view;
        battleView.SwitchToUpdatingStateChange(this);

        currentAbilityIndex = 0;

        AnimationTrigger animation = abilitySelections[currentAbilityIndex].Ability.Profile.Animation;
        playerFigure = view.PlayerFigureGroup.GetFigure(player);
        playerFigure.PlayAnimation(animation, FinishAbilityAnimation, ReturnToIdleAndFinishedSequence);
    }

    public override void Update(BattleView view, BattleController controller)
    {

    }

    public void ReturnToIdleAndFinishedSequence()
    {
        if (currentAbilityIndex >= abilitySelections.Count)
        {
            battleView.SwitchToParsing();
        }
    }

    public void FinishAbilityAnimation()
    {
        battleView.BattleStatsPanel.PlayerStatPanel.UpdateStats(results[currentAbilityIndex].PlayerStatsAfter);
        battleView.BattleStatsPanel.UpdateAllEnemyStats(results[currentAbilityIndex].EnemyStatsAfter);
        currentAbilityIndex++;
        if (currentAbilityIndex < abilitySelections.Count)
        {
            AnimationTrigger animation = abilitySelections[currentAbilityIndex].Ability.Profile.Animation;
            playerFigure.PlayAnimation(animation, FinishAbilityAnimation, ReturnToIdleAndFinishedSequence);
        }
    }
}
