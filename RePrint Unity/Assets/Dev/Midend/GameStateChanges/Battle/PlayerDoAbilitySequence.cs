using System.Collections.Generic;
using UnityEngine;


public class PlayerDoAbilitySequence : BattleStateChange
{

    private List<StatChangeBreakdown> statChangeBreakdowns;

    private List<AbilitySelection> abilitySelections;

    private Character player;
    private CharacterFigure playerFigure;

    private int currentAbilityIndex;

    private bool sequenceEnded;

    private float timeAfterSequenceEnded;

    private bool sequenceStarted;

    private float timeBeforeStartingSequence;


    private BattleView battleView;

    public PlayerDoAbilitySequence(Character _player, List<AbilitySelection> _abilitySelections, List<StatChangeBreakdown> _statChangeBreakdowns)
    {
        player = _player;
        abilitySelections = _abilitySelections;
        statChangeBreakdowns = _statChangeBreakdowns;

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
            battleView.BattleStatsPanel.PlayerStatPanel.UpdateStats(player.Stats);
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
        battleView.BattleStatsPanel.PlayerStatPanel.UpdateStats(statChangeBreakdowns[currentAbilityIndex].statsAfter.PlayerStats);
        battleView.BattleStatsPanel.UpdateAllEnemyStats(statChangeBreakdowns[currentAbilityIndex].statsAfter.EnemyStats);

        foreach (Character enemy in statChangeBreakdowns[currentAbilityIndex].statsAfter.EnemyStats.Keys)
        {
            battleView.EnemyFigureGroup.GetFigure(enemy).UpdateStats(statChangeBreakdowns[currentAbilityIndex].statsAfter.EnemyStats[enemy]);
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
