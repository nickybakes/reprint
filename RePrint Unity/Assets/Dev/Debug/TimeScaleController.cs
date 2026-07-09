using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeScaleController : MonoBehaviour
{
    [SerializeField] private float[] timeScales = new float[] { 0.1f, 0.25f, 0.5f, 0.75f, 1f, 2f, 5f, 10f };

    private int currentTimeScaleIndex = 4;

    void Update()
    {
        if (Keyboard.current[Key.Equals].wasPressedThisFrame || Keyboard.current[Key.NumpadPlus].wasPressedThisFrame)
        {
            ChangeSpeed(1);
        }
        else if (Keyboard.current[Key.Minus].wasPressedThisFrame || Keyboard.current[Key.NumpadMinus].wasPressedThisFrame)
        {
            ChangeSpeed(-1);
        }
        else if (Keyboard.current[Key.Digit0].wasPressedThisFrame)
        {
            ResetSpeed();
        }
        else if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            PauseGame();
        }
    }

    public void ChangeSpeed(int change)
    {
        currentTimeScaleIndex = Math.Clamp(currentTimeScaleIndex + change, 0, timeScales.Length - 1);

        Time.timeScale = timeScales[currentTimeScaleIndex];
    }

    public void PauseGame()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            ChangeSpeed(0);
        }
    }

    public void ResetSpeed()
    {
        currentTimeScaleIndex = 4;
        ChangeSpeed(0);
    }
}
