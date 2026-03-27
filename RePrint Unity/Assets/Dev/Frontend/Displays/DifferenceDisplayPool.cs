using System.Collections.Generic;
using UnityEngine;

public class DifferenceDisplayPool : Display
{

    /// <summary>
    /// The Text Display prefab to spawn.
    /// </summary>
    [SerializeField] private DifferenceDisplay prefab;

    public void AddText(int a, int b)
    {
        DifferenceDisplay display = Instantiate(prefab, transform);
        display.Display(a, b);
    }


    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }
}
