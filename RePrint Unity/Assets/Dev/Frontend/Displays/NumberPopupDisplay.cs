using System;
using TMPro;
using UnityEngine;

public class NumberPopupDisplay : Display
{

    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Animator animator;

    public float CurrentTime { get => currentTime; }

    public bool IsShowing { get; private set; }

    private float currentTime;

    private bool inIntro;

    private bool inOutro;

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }


    public void Hide()
    {
        IsShowing = false;
    }


    public void Display(string content, float introDelay = 0)
    {
        IsShowing = true;
        contentText.text = content;
        currentTime = 0;
        inOutro = false;
        inIntro = true;

        if (introDelay > 0)
        {
            currentTime = -introDelay;
        }
    }

    // Returns true if display has become available on this frame
    public bool UpdateLifetime(float lifetime, float outroLength)
    {
        currentTime += Time.deltaTime;

        if (inIntro && !inOutro && currentTime >= 0)
        {
            inIntro = false;
            animator.SetTrigger("Intro");
        }
        else if (currentTime >= lifetime && !inOutro && !inIntro)
        {
            inOutro = true;
            animator.SetTrigger("Outro");
        }
        else if (inOutro && !inIntro && currentTime >= lifetime + outroLength)
        {
            IsShowing = false;
            return true;
        }
        return false;
    }
}
