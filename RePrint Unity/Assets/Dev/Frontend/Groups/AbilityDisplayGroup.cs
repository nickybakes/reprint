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

    [SerializeField] private Vector2 abilityStatsGroupOffset;
    [SerializeField] private Vector2 abilityStatsGroupOffsetExpanded;

    [SerializeField] private Display goButton;

    [SerializeField] private Vector2 goButtonOffset;
    [SerializeField] private Vector2 goButtonOffsetExpanded;

    [HideInInspector] public Display abilityStatsGroup;
    [SerializeField] private SFXData cancelSound;


    private Dictionary<Ability, AbilityDisplay> abilityToDisplayReferences;
    private Dictionary<AbilityDisplay, Ability> displayToAbilityReferences;

    private AbilityDisplay firstAbilityDisplay;
    private AbilityDisplay lastAbilityDisplay;

    private bool abilitiesAdded;

    private BattleController controller;

    public void AddAbilities(List<PlayerAbility> _abilities, BattleController _controller)
    {
        abilityToDisplayReferences = new Dictionary<Ability, AbilityDisplay>();
        displayToAbilityReferences = new Dictionary<AbilityDisplay, Ability>();
        controller = _controller;
        for (int i = 0; i < _abilities.Count; i++)
        {
            Ability ability = _abilities[i];
            AbilityDisplay display = Instantiate(abilityDisplayPrefab, spawnParent);
            display.DisplayAbility(ability, controller);
            abilityToDisplayReferences.Add(ability, display);
            displayToAbilityReferences.Add(display, ability);
            group.AddDisplayToGroup(display);

            if (i == 0)
            {
                firstAbilityDisplay = display;
            }

            if (i == _abilities.Count - 1)
            {
                lastAbilityDisplay = display;
            }
        }
        abilitiesAdded = true;
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

    void Update()
    {
        if (abilitiesAdded && goButton)
        {
            goButton.GetRect().anchoredPosition = lastAbilityDisplay.GetRect().anchoredPosition + Vector2.Lerp(goButtonOffset, goButtonOffsetExpanded, lastAbilityDisplay.SizeTransition);
        }

        if (abilitiesAdded && abilityStatsGroup)
        {
            abilityStatsGroup.GetRect().anchoredPosition = firstAbilityDisplay.GetRect().anchoredPosition + Vector2.Lerp(abilityStatsGroupOffset, abilityStatsGroupOffsetExpanded, firstAbilityDisplay.SizeTransition) - abilityStatsGroup.GetParentRect().anchoredPosition;
        }
    }
}
