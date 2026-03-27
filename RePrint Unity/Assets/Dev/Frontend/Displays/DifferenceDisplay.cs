using System;
using UnityEngine;

public class DifferenceDisplay : Display
{
    [SerializeField] private TextDisplay textDisplay;
    [SerializeField] private float lifeTime = .5f;
    [SerializeField] private float fadeTime = .1f;
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
        if (a >= b)
        {
            textDisplay.SetText("-" + difference);
        }
        else
        {
            textDisplay.SetText("+" + difference);
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
