using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplayGroup : MonoBehaviour
{

    [SerializeField] private FloatingDisplayGroup group;

    /// <summary>
    /// The parent to spawn the prefabs under.
    /// </summary>
    [SerializeField] private Transform spawnParent;

    [SerializeField] private AbilityDisplay abilityDisplayPrefab;

    private Dictionary<Ability, AbilityDisplay> abilityToDisplayReferences;
    private Dictionary<AbilityDisplay, Ability> displayToAbilityReferences;

    private BattleController controller;

    public void AddAbilities(List<PlayerAbility> _abilities, BattleController _controller)
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

    public void ResetSequenceState(CharacterStats stats)
    {
        foreach (AbilityDisplay display in abilityToDisplayReferences.Values)
        {
            display.RefreshSequenceState(false, -1, stats.Character, stats.AbilityPoints, 0);
        }
    }

    public void RefreshSequenceState(AbilitySequence abilitySequence, CharacterStats stats)
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
                        display.RefreshSequenceState(true, lastSelection.Overclock, stats.Character, stats.AbilityPoints, lastSelection.Ability.GetAPCost(lastSelection.Overclock));
                    }
                    else
                    {
                        display.RefreshSequenceState(false, -1, stats.Character, stats.AbilityPoints, lastSelection.Ability.GetAPCost(lastSelection.Overclock));
                    }
                }
                else
                {
                    display.RefreshSequenceState(false, -1, stats.Character, stats.AbilityPoints, 0);
                }
            }
        }
        else
        {
            ResetSequenceState(stats);
        }
    }

    public void Hide()
    {
        foreach (AbilityDisplay display in abilityToDisplayReferences.Values)
        {
            display.Hide();
        }
    }

    public void Show()
    {
        foreach (AbilityDisplay display in abilityToDisplayReferences.Values)
        {
            display.Show();
        }
    }
}
