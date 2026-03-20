using System;
using System.Collections.Generic;

public class AbilitySequence
{
    public static int MAX_OVERCLOCK = 4;

    private List<AbilitySelection> sequence;

    public AbilitySequence()
    {
        sequence = new List<AbilitySelection>();
    }

    public void AddOrOverclockAbility(Ability ability)
    {
        if (sequence.Count == 0 || !TryOverclockLastAbility(ability))
        {
            sequence.Add(new AbilitySelection(ability));
        }
    }

    public void SetLastAbilityTarget(Character target)
    {
        if (sequence.Count == 0)
            return;

        AbilitySelection lastAbility = GetLastSelection();
        lastAbility.target = target;
    }

    public bool StepBackInSequenceBuilding()
    {
        if (sequence.Count == 0)
            return false;

        AbilitySelection lastAbility = GetLastSelection();

        if (lastAbility.target == null)
        {
            lastAbility.target = null;
        }
        else if (lastAbility.overclock == 0)
        {
            sequence.RemoveAt(sequence.Count - 1);
        }
        else
        {
            lastAbility.Underclock();
        }

        return true;
    }

    private AbilitySelection GetLastSelection()
    {
        return sequence[sequence.Count - 1];
    }

    private bool TryOverclockLastAbility(Ability newAbility)
    {
        if (sequence.Count == 0)
            return false;

        AbilitySelection lastSelection = GetLastSelection();

        if (lastSelection.target == null && lastSelection.ability != newAbility)
        {
            lastSelection.Overclock();
            return true;
        }

        return false;
    }
}