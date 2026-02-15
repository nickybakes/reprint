using System;
using System.Collections.Generic;
using UnityEngine;

enum ActionSequenceCreationState
{
    PickAction,

    PickTarget,
}

public class BattleStatePlayerCreateActionSequence : BattleState
{

    public static int MAX_OVERCLOCK = 4;


    private List<SelectedAction> selectedActionSequence;

    public List<SelectedAction> SelectedActionSequence
    {
        get
        {
            return selectedActionSequence;
        }
    }

    public void SubmitAction(int actionIndex)
    {
        if (selectedActionSequence.Count == 0)
        {
            AddNewActionToSequence(actionIndex);
        }
        else
        {
            SelectedAction action = selectedActionSequence[selectedActionSequence.Count - 1];

            if (action.enemyIndex == -1)
            {
                if (action.actionIndex == actionIndex)
                {
                    action.overclock = Math.Min(action.overclock + 1, MAX_OVERCLOCK);
                }
                else
                {
                    action.actionIndex = actionIndex;
                    action.overclock = 0;
                }
            }
            else
            {
                AddNewActionToSequence(actionIndex);
            }

        }

        BattleManager.battle.ui.RefreshPlayerActionMenu();
    }

    public void SubmitEnemySelection(int enemyIndex)
    {
        if (selectedActionSequence.Count == 0)
            return;

        SelectedAction action = selectedActionSequence[selectedActionSequence.Count - 1];
        action.enemyIndex = enemyIndex;

        BattleManager.battle.ui.RefreshPlayerActionMenu();
    }

    private void AddNewActionToSequence(int index)
    {
        SelectedAction newAction = new SelectedAction();
        newAction.actionIndex = index;
        selectedActionSequence.Add(newAction);
    }

    public List<SelectedAction> GetTrimmedSequence()
    {
        if (selectedActionSequence.Count > 0 && selectedActionSequence[selectedActionSequence.Count - 1].enemyIndex == -1)
        {
            return selectedActionSequence.GetRange(0, selectedActionSequence.Count - 1);
        }

        return selectedActionSequence;
    }

    public override void StartState()
    {
        base.StartState();
        selectedActionSequence = new List<SelectedAction>();
        BattleManager.battle.ui.OpenPlayerActionMenu();
        BattleManager.battle.ui.OpenActionSequencePanel();
    }

    public override void EndState()
    {
        base.EndState();
        BattleManager.battle.ui.ClosePlayerActionMenu();
        BattleManager.battle.ui.CloseActionSequencePanel();
    }

    public void Back()
    {
        if (selectedActionSequence.Count == 0)
            return;

        SelectedAction action = selectedActionSequence[selectedActionSequence.Count - 1];

        if (action.enemyIndex != -1)
        {
            action.enemyIndex = -1;
        }
        else if (action.overclock == 0)
        {
            selectedActionSequence.RemoveAt(selectedActionSequence.Count - 1);
        }
        else
        {
            action.overclock = Math.Max(action.overclock - 1, 0);
        }


        BattleManager.battle.ui.RefreshPlayerActionMenu();
    }
}

public class SelectedAction
{
    public int actionIndex;

    public int overclock = 0;

    public int enemyIndex = -1;
}