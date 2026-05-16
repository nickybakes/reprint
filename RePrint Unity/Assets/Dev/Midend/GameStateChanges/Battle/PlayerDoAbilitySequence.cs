using System.Collections.Generic;
using UnityEngine;


public class PlayerDoAbilitySequence : BattleStateChange
{

    private List<AbilityResults> results;

    private List<AbilitySelection> abilitySelections;

    private Character player;
    private CharacterFigure playerFigure;

    private int currentAbilityIndex;

    private bool sequenceEnded;

    private float timeAfterSequenceEnded;

    private bool sequenceStarted;

    private float timeBeforeStartingSequence;


    private BattleView battleView;

    public PlayerDoAbilitySequence(Character _player, List<AbilitySelection> _abilitySelections, List<AbilityResults> _results)
    {
        player = _player;
        abilitySelections = _abilitySelections;
        results = _results;

        sequenceEnded = false;
        sequenceStarted = false;
    }

    public override void ParseChange(BattleView view, BattleController controller)
    {
        battleView = view;
        battleView.SwitchToUpdatingStateChange(this);

        playerFigure = view.PlayerFigureGroup.GetFigure(player);

        view.BattleStatsPanel.PlayerStatPanel.Hide();
    }

    public override void Update(BattleView view, BattleController controller)
    {
        if (!sequenceStarted)
        {
            timeBeforeStartingSequence += Time.deltaTime;
            if (timeBeforeStartingSequence > view.BattleTimingProfile.TimeBeforePlayerAbilityAnimationSequence)
            {
                currentAbilityIndex = 0;
                sequenceStarted = true;
                PlayCurrentAnimation();
            }
        }

        if (sequenceEnded)
        {
            timeAfterSequenceEnded += Time.deltaTime;
            if (timeAfterSequenceEnded >= view.BattleTimingProfile.TimeAfterPlayerAbilityAnimationSequence)
            {
                battleView.SwitchToParsing();
            }
        }
    }

    public void ReturnToIdleAndFinishedSequence()
    {
        if (currentAbilityIndex >= abilitySelections.Count)
        {
            sequenceEnded = true;
            battleView.BattleStatsPanel.PlayerStatPanel.Show();
        }
    }

    public void FinishAbilityAnimation()
    {
        currentAbilityIndex++;
        if (currentAbilityIndex < abilitySelections.Count)
        {
            PlayCurrentAnimation();
        }
    }

    public void UpdateStats()
    {
        battleView.BattleStatsPanel.PlayerStatPanel.UpdateStats(results[currentAbilityIndex].PlayerStatsAfter);
        battleView.BattleStatsPanel.UpdateAllEnemyStats(results[currentAbilityIndex].EnemyStatsAfter);

        foreach (Character enemy in results[currentAbilityIndex].EnemyStatsAfter.Keys)
        {
            battleView.EnemyFigureGroup.GetFigure(enemy).UpdateStats(results[currentAbilityIndex].EnemyStatsAfter[enemy]);
            battleView.CameraManager.Shake(1);
        }
    }

    private void PlayCurrentAnimation()
    {
        AnimationTrigger animation = abilitySelections[currentAbilityIndex].Ability.Profile.Animation;
        CharacterFigure targretFigure = battleView.EnemyFigureGroup.GetFigure(abilitySelections[currentAbilityIndex].Target);

        if (abilitySelections[currentAbilityIndex].Target == player)
        {
            targretFigure = battleView.PlayerFigureGroup.GetFigure(player);
        }

        playerFigure.PlayAbilityAnimation(animation, FinishAbilityAnimation, ReturnToIdleAndFinishedSequence, UpdateStats, targretFigure);
    }
}
