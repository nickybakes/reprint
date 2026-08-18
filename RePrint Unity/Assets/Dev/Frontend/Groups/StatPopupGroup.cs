using System;
using System.Collections.Generic;
using UnityEngine;

public class StatPopupGroup : MonoBehaviour
{
    [SerializeField] private StatPopupDisplay prefab;
    [SerializeField] private int displayAmount = 3;
    [SerializeField] private float introDelay = .25f;
    [SerializeField] private float lifetime = 2;
    [SerializeField] private float outroLength = .5f;

    [Header("Use %a for the value, %b for the plus/minus sign, and %c for the stat type")]
    [SerializeField] private string valueFormat = "%c: %b%a";

    [SerializeField] private string dodgeString = "Dodge";
    [SerializeField] private string chainString = "Chain";

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
            display.Hide();
            displays.Add(display);
        }
    }

    public void DisplayStat(string name, StatType statType, float amount)
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
                        currentIntroDelay += introDelay;
                        queue.RemoveAt(0);
                    }
                }
            }
        }

        currentIntroDelay = Mathf.Max(0, currentIntroDelay - Time.deltaTime);
    }
}
