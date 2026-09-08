using System;
using System.Collections.Generic;
using UnityEngine;

public enum NumberPopupType
{
    PhysicalDamage,
    CritDamage,
}

public class NumberPopupGroup : MonoBehaviour
{
    [SerializeField] private NumberPopupDisplay physicalDamagePrefab;
    [SerializeField] private NumberPopupDisplay critDamagePrefab;

    [SerializeField] private Transform physicalDamagePosition;
    [SerializeField] private Transform critDamagePosition;


    [SerializeField] private NumberPopupDisplay prefab;
    [SerializeField] private int displayAmount = 3;
    [SerializeField] private Transform gamePanel;
    [SerializeField] private float introDelay = .25f;
    [SerializeField] private float lifetime = 1;
    [SerializeField] private float outroLength = .5f;

    [Header("Use %a for the value and %b for the plus/minus sign")]
    [SerializeField] private string physicalDamageFormat = "%b%a";
    [SerializeField] private string critDamageFormat = "%b%a!";

    private List<NumberPopupDisplay> displays;

    private float currentIntroDelay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        displays = new List<NumberPopupDisplay>();

        for (int i = 0; i < displayAmount; i++)
        {
            NumberPopupDisplay display = Instantiate(prefab, transform);
            display.gameObject.SetActive(true);
            display.Hide();
            displays.Add(display);
        }
    }

    public void DisplayNumber(NumberPopupType type, float amount, bool isMultiplicative = false)
    {
        int sign = Math.Sign(amount);
        float absAmount = Mathf.Abs(amount);
        string finalString = physicalDamageFormat;

        switch (type)
        {
            case NumberPopupType.CritDamage:
                finalString = critDamageFormat;
                break;
        }

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

        // TODO: Maybe add the subtag parsing to this for sprites n such

        Display(finalString);
    }

    private void Display(string content)
    {
        int displayIndex = -1;
        int oldestShowingDisplayIndex = 0;

        for (int i = 0; i < displays.Count; i++)
        {
            if (!displays[i].IsShowing)
            {
                displayIndex = i;
                break;
            }
            else if (displays[i].CurrentTime > displays[oldestShowingDisplayIndex].CurrentTime)
            {
                oldestShowingDisplayIndex = i;
            }
        }

        if (displayIndex == -1)
        {
            displayIndex = oldestShowingDisplayIndex;
        }

        displays[displayIndex].Display(content, currentIntroDelay);
        NonParentModePositioning(displayIndex);

        currentIntroDelay += introDelay;
    }

    void Update()
    {
        for (int i = 0; i < displays.Count; i++)
        {
            if (displays[i].IsShowing)
            {
                displays[i].UpdateLifetime(lifetime, outroLength);
            }
        }

        currentIntroDelay = Mathf.Max(0, currentIntroDelay - Time.deltaTime);
    }

    void NonParentModePositioning(int displayIndex)
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
