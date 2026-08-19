using System;
using System.Collections.Generic;
using UnityEngine;

public class StatPopupGroup : MonoBehaviour
{
    [SerializeField] private StatPopupDisplay prefab;
    [SerializeField] private int displayAmount = 3;
    [SerializeField] private bool nonParentedMode;
    [SerializeField] private Transform gamePanel;
    [SerializeField] private float introDelay = .25f;
    [SerializeField] private float lifetime = 2;
    [SerializeField] private float outroLength = .5f;

    [Header("Use %a for the value, %b for the plus/minus sign, and %c for the stat type")]
    [SerializeField] private string valueFormat = "%c: %b%a";

    [SerializeField] private string dodgeString = "Dodge";
    [SerializeField] private string chainString = "Chain";
    [SerializeField] private string maxAPString = "Max AP";

    [Header("Use %a for the ability name")]
    [SerializeField] private string retriggerFormat = "Retrigger: %a";


    private List<StatPopupDisplay> displays;

    private List<Tuple<string, string>> queue;

    private float currentIntroDelay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        queue = new List<Tuple<string, string>>();
        displays = new List<StatPopupDisplay>();

        for (int i = 0; i < displayAmount; i++)
        {
            StatPopupDisplay display = Instantiate(prefab, transform);
            display.gameObject.SetActive(true);
            display.Hide();
            displays.Add(display);
        }
    }

    public void DisplayModResults(List<ModResult> modResults, Character player)
    {
        foreach (ModResult modResult in modResults)
        {
            DisplayStatChangeAmounts(modResult.mod.Profile.Name, modResult.statChangeAmounts, player);
        }
    }

    public void DisplayAbilityResults(Ability ability, StatChangeBreakdown statChangeBreakdown, Character player)
    {
        DisplayStatChangeAmounts(ability.Profile.Name, statChangeBreakdown.abilityStatChanges, player);
    }

    private void DisplayStatChangeAmounts(string name, StatChangeAmounts statChangeAmounts, Character character)
    {
        float chainValue = statChangeAmounts.GetAmount(character, StatChange.ChainGained);
        if (chainValue != 0)
        {
            DisplayStat(name, StatType.Chain, chainValue);
        }

        float dodgeValue = statChangeAmounts.GetAmount(character, StatChange.DodgeGained);
        if (dodgeValue != 0)
        {
            DisplayStat(name, StatType.Dodge, dodgeValue);
        }

        float maxAPValue = statChangeAmounts.GetAmount(character, StatChange.APMaxIncrease);
        if (maxAPValue != 0)
        {
            DisplayStat(name, StatType.MaxAP, maxAPValue);
        }
    }

    public void DisplayRetrigger(string name, AbilitySelection abilitySelection)
    {
        string finalString = retriggerFormat;
        string abilityName = abilitySelection.Ability.Profile.Name;

        while (finalString.Contains("%a"))
        {
            finalString = finalString.Replace("%a", abilityName);
        }

        Display(name, finalString);
    }

    public void DisplayStat(string name, StatType statType, float amount, bool isMultiplicative = false)
    {
        int sign = Math.Sign(amount);
        float absAmount = Mathf.Abs(amount);
        string finalString = valueFormat;
        while (finalString.Contains("%a"))
        {
            finalString = finalString.Replace("%a", absAmount.ToString());
        }

        string signString = "";
        if (sign < 0)
            signString = "-";
        else if (sign > 0)
            signString = "+";

        if (isMultiplicative)
        {
            signString = "x";
        }

        while (finalString.Contains("%b"))
        {
            finalString = finalString.Replace("%b", signString);
        }

        string statString = "Stat";

        switch (statType)
        {
            case StatType.Dodge:
                statString = dodgeString;
                break;
            case StatType.Chain:
                statString = chainString;
                break;
            case StatType.MaxAP:
                statString = maxAPString;
                break;
        }

        while (finalString.Contains("%c"))
        {
            finalString = finalString.Replace("%c", statString);
        }

        Display(name, finalString);
    }

    private void Display(string name, string content)
    {
        int displayIndex = -1;

        for (int i = 0; i < displays.Count; i++)
        {
            if (!displays[i].IsShowing)
            {
                displayIndex = i;
                break;
            }
        }

        if (displayIndex == -1)
        {
            queue.Add(new Tuple<string, string>(name, content));
        }
        else
        {
            displays[displayIndex].Display(name, content, currentIntroDelay);
            NonParentModePositioning(displayIndex);

            currentIntroDelay += introDelay;
        }
    }

    void Update()
    {
        for (int i = 0; i < displays.Count; i++)
        {
            if (displays[i].IsShowing)
            {
                if (displays[i].UpdateLifetime(lifetime, outroLength))
                {
                    if (queue.Count > 0)
                    {
                        displays[i].Display(queue[0].Item1, queue[0].Item2, currentIntroDelay);
                        NonParentModePositioning(i);
                        currentIntroDelay += introDelay;
                        queue.RemoveAt(0);
                    }
                }
            }
        }

        currentIntroDelay = Mathf.Max(0, currentIntroDelay - Time.deltaTime);
    }

    void NonParentModePositioning(int displayIndex)
    {
        if (nonParentedMode)
        {
            displays[displayIndex].transform.SetParent(transform);
            displays[displayIndex].transform.localPosition = Vector3.zero;
            displays[displayIndex].transform.localRotation = Quaternion.identity;
            displays[displayIndex].transform.localScale = Vector3.one;
            displays[displayIndex].transform.SetParent(gamePanel);

            Vector2 currentAnchoredPosition = displays[displayIndex].GetRect().anchoredPosition;

            for (int i = 0; i < displays.Count; i++)
            {
                if (displays[i].IsShowing && i != displayIndex)
                {
                    if (Vector2.Distance(displays[i].GetRect().anchoredPosition, displays[displayIndex].GetRect().anchoredPosition) <= displays[displayIndex].GetRect().rect.height)
                    {
                        currentAnchoredPosition.y += displays[displayIndex].GetRect().rect.height;
                    }
                }
            }

            displays[displayIndex].GetRect().anchoredPosition = currentAnchoredPosition;
        }
    }
}
