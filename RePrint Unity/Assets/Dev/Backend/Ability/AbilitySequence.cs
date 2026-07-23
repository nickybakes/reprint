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

    public int GetTotalOverclock()
    {
        int total = 0;

        foreach (AbilitySelection selection in sequence)
        {
            total += selection.Overclock;
        }

        return total;
    }

    public AbilitySequenceChangeType AddOrOverclockAbility(Ability ability, int availableAP)
    {
        AbilitySequenceChangeType changeType = TryOverclockOrChangeLastAbility(ability, availableAP);
        if (changeType == AbilitySequenceChangeType.None && availableAP >= ability.GetAPCost(0))
        {
            sequence.Add(new AbilitySelection(ability));
            return AbilitySequenceChangeType.SubmitAbility;
        }

        return changeType;
    }

    public AbilitySequenceChangeType SetLastAbilityTarget(Character target)
    {
        if (sequence.Count == 0)
            return AbilitySequenceChangeType.None;

        AbilitySelection lastAbility = GetLastSelection();
        lastAbility.SetTarget(target);
        return AbilitySequenceChangeType.SubmitTarget;
    }

    public AbilitySequenceChangeType StepBackInSequenceBuilding()
    {
        if (sequence.Count == 0)
            return AbilitySequenceChangeType.None;

        AbilitySelection lastAbility = GetLastSelection();

        if (lastAbility.TargetIsSet)
        {
            lastAbility.UnsetTarget();
            return AbilitySequenceChangeType.UnsubmitTarget;
        }
        else if (lastAbility.Overclock == 0)
        {
            sequence.RemoveAt(sequence.Count - 1);
            return AbilitySequenceChangeType.UnsubmitAbility;
        }
        else
        {
            lastAbility.DecreaseOverclock();
            return AbilitySequenceChangeType.DecreaseOverclock;
        }
    }

    public List<AbilitySelection> GetSortedSequence()
    {
        List<AbilitySelection> sortedSequence = new List<AbilitySelection>();

        // For now, the sorted sequence moves Utility abilities to the front
        // of the sequence. Might want to change this later to a priority value system.

        foreach (AbilitySelection abilitySelection in sequence)
        {
            if (abilitySelection.Ability.Type == AbilityType.Utility)
            {
                sortedSequence.Add(abilitySelection);
            }
        }

        foreach (AbilitySelection abilitySelection in sequence)
        {
            if (abilitySelection.Ability.Type != AbilityType.Utility)
            {
                sortedSequence.Add(abilitySelection);
            }
        }

        return sortedSequence;
    }

    public AbilitySelection GetLastSelection()
    {
        return sequence[sequence.Count - 1];
    }

    private AbilitySequenceChangeType TryOverclockOrChangeLastAbility(Ability newAbility, int availableAP)
    {
        if (sequence.Count == 0)
            return AbilitySequenceChangeType.None;

        AbilitySelection lastSelection = GetLastSelection();

        if (!lastSelection.TargetIsSet)
        {
            if (lastSelection.Ability == newAbility)
            {
                if (lastSelection.Overclock < Ability.MAX_OVERCLOCK)
                {
                    int nextAPCost = lastSelection.Ability.GetAPCost(lastSelection.Overclock + 1);
                    int currAPCost = lastSelection.Ability.GetAPCost(lastSelection.Overclock);
                    if (availableAP + currAPCost >= nextAPCost)
                    {
                        lastSelection.IncreaseOverclock();
                        return AbilitySequenceChangeType.IncreaseOverclock;
                    }
                    else
                    {
                        return AbilitySequenceChangeType.None;
                    }
                }
            }
            else
            {
                int currAPCost = lastSelection.Ability.GetAPCost(lastSelection.Overclock);
                if (availableAP + currAPCost >= newAbility.GetAPCost(0))
                {
                    lastSelection.SetAbility(newAbility);
                    return AbilitySequenceChangeType.SubmitAbility;
                }
            }
        }

        return AbilitySequenceChangeType.None;
    }
}