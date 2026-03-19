using System.Collections.Generic;

public class AbilitySequence
{
    public static int MAX_OVERCLOCK = 4;

    private List<AbilitySelection> sequence;

    public AbilitySequence()
    {
        sequence = new List<AbilitySelection>();
    }

    public void AddAbility(Ability ability)
    {
        sequence.Add(new AbilitySelection(ability));
    }

    public void SetLastAbilityTarget(Character target)
    {

    }
}