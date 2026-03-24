using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplayGroup : MonoBehaviour
{

    [SerializeField] private FloatingDisplayGroup group;

    /// <summary>
    /// The parent to spawn the prefabs under.
    /// </summary>
    [SerializeField] private Transform spawnParent;

    // [SerializeField] private Transform actionStatsDisplay;

    [SerializeField] private AbilityDisplay abilityDisplayPrefab;

    private Dictionary<Ability, AbilityDisplay> abilityToDisplayReferences;
    private Dictionary<AbilityDisplay, Ability> displayToAbilityReferences;

    private BattleController controller;

    public void AddAbilities(List<Ability> _abilities, BattleController _controller)
    {
        abilityToDisplayReferences = new Dictionary<Ability, AbilityDisplay>();
        displayToAbilityReferences = new Dictionary<AbilityDisplay, Ability>();
        controller = _controller;
        foreach (Ability ability in _abilities)
        {
            AbilityDisplay display = Instantiate(abilityDisplayPrefab, spawnParent);
            display.DisplayAbility(ability, controller);
            abilityToDisplayReferences.Add(ability, display);
            displayToAbilityReferences.Add(display, ability);
            group.AddDisplayToGroup(display);
        }
    }

    public void ResetSequenceState()
    {
        foreach (AbilityDisplay display in abilityToDisplayReferences.Values)
        {
            display.RefreshSequenceState(false, -1);
        }
    }

    public void RefreshSequenceState(AbilitySequence abilitySequence)
    {
        List<AbilitySelection> sequence = abilitySequence.Sequence;

        if (sequence.Count > 0)
        {
            AbilitySelection lastSelection = abilitySequence.GetLastSelection();

            foreach (AbilityDisplay display in displayToAbilityReferences.Keys)
            {
                Ability ability = displayToAbilityReferences.GetValueOrDefault(display);

                if (!lastSelection.TargetIsSet)
                {
                    if (lastSelection.Ability == ability)
                    {
                        display.RefreshSequenceState(true, lastSelection.Overclock);
                    }
                    else
                    {
                        display.RefreshSequenceState(false, -1);
                    }
                }
            }
        }
        else
        {
            ResetSequenceState();
        }
    }
}
