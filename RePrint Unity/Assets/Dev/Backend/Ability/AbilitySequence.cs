using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySequence
{
    public static int MAX_OVERCLOCK = 4;

    private List<AbilitySelection> sequence;

    public List<AbilitySelection> Sequence { get => sequence; }

    public AbilitySequence()
    {
        sequence = new List<AbilitySelection>();
    }

    public void AddOrOverclockAbility(Ability ability)
    {
        if (sequence.Count == 0 || !TryOverclockOrChangeLastAbility(ability))
        {
            sequence.Add(new AbilitySelection(ability));
        }
    }

    public void SetLastAbilityTarget(Character target)
    {
        if (sequence.Count == 0)
            return;

        AbilitySelection lastAbility = GetLastSelection();
        lastAbility.SetTarget(target);
    }

    public bool StepBackInSequenceBuilding()
    {
        if (sequence.Count == 0)
            return false;

        AbilitySelection lastAbility = GetLastSelection();

        if (lastAbility.TargetIsSet)
        {
            lastAbility.UnsetTarget();
        }
        else if (lastAbility.Overclock == 0)
        {
            sequence.RemoveAt(sequence.Count - 1);
        }
        else
        {
            lastAbility.DecreaseOverclock();
        }

        return true;
    }

    public AbilitySelection GetLastSelection()
    {
        return sequence[sequence.Count - 1];
    }

    private bool TryOverclockOrChangeLastAbility(Ability newAbility)
    {
        if (sequence.Count == 0)
            return false;

        AbilitySelection lastSelection = GetLastSelection();

        if (!lastSelection.TargetIsSet)
        {
            if (lastSelection.Ability == newAbility)
            {
                lastSelection.IncreaseOverclock();
            }
            else
            {
                lastSelection.SetAbility(newAbility);
            }
            return true;
        }

        return false;
    }
}