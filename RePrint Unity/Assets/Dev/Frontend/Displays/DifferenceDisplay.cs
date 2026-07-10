using System;
using System.ComponentModel;
using UnityEngine;

public class DifferenceDisplay : Display
{
    [SerializeField] private TextDisplay textDisplay;
    [SerializeField] private bool haveTextDisplayBump;
    [SerializeField] private float lifeTime = .5f;
    [SerializeField] private float fadeTime = .1f;

    [Header("Use %a for the value, and %b for the plus/minus sign.")]
    [SerializeField] private string valueFormat = "%a";

    [Header("Movement")]
    [SerializeField] private Vector2 startingVelocityMin;
    [SerializeField] private Vector2 startingVelocityMax;
    [SerializeField] private float gravity = -30f;

    private float currentTime;
    private Vector2 velocity;

    private bool fading;

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    public void Display(int a, int b)
    {
        int difference = Math.Abs(a - b);
        string finalString = valueFormat;
        while (finalString.Contains("%a"))
        {
            finalString = finalString.Replace("%a", difference.ToString());
        }

        string signString = "";
        if (a > b)
            signString = "-";
        else if (a < b)
            signString = "+";

        while (finalString.Contains("%b"))
        {
            finalString = finalString.Replace("%b", signString);
        }

        if (haveTextDisplayBump)
        {
            textDisplay.SetText(finalString, true);
        }
        else
        {
            textDisplay.SetTextNoBump(finalString);
        }

        currentTime = 0;
        velocity = new Vector2
        {
            x = UnityEngine.Random.Range(startingVelocityMin.x, startingVelocityMax.x),
            y = UnityEngine.Random.Range(startingVelocityMin.y, startingVelocityMax.y)
        };
    }

    void Update()
    {
        velocity.y += gravity * Time.deltaTime;

        rectTransform.anchoredPosition += velocity * Time.deltaTime;

        currentTime += Time.deltaTime;

        if (!fading && currentTime >= lifeTime - fadeTime)
        {
            textDisplay.Hide();
            fading = true;
        }

        if (currentTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

}
