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

    public void AddOrOverclockAbility(Ability ability, int availableAP)
    {
        if (sequence.Count == 0 || !TryOverclockOrChangeLastAbility(ability, availableAP))
        {
            if (availableAP >= ability.GetAbilityRules(0).APCost)
            {
                sequence.Add(new AbilitySelection(ability));
            }
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

    private bool TryOverclockOrChangeLastAbility(Ability newAbility, int availableAP)
    {
        if (sequence.Count == 0)
            return false;

        AbilitySelection lastSelection = GetLastSelection();

        if (!lastSelection.TargetIsSet)
        {
            if (lastSelection.Ability == newAbility)
            {
                if (lastSelection.Overclock < Ability.MAX_OVERCLOCK)
                {
                    int nextAPCost = lastSelection.Ability.GetAbilityRules(lastSelection.Overclock + 1).APCost;
                    int currAPCost = lastSelection.Ability.GetAbilityRules(lastSelection.Overclock).APCost;
                    if (availableAP + currAPCost >= nextAPCost)
                    {
                        lastSelection.IncreaseOverclock();
                    }
                }
            }
            else
            {
                int currAPCost = lastSelection.Ability.GetAbilityRules(lastSelection.Overclock).APCost;
                if (availableAP + currAPCost >= newAbility.GetAbilityRules(0).APCost)
                {
                    lastSelection.SetAbility(newAbility);
                }
            }
            return true;
        }

        return false;
    }
}