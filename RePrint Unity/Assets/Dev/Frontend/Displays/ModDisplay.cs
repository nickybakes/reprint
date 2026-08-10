using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModDisplay : Display
{

    [SerializeField] private Image backgroundImage;

    [SerializeField] private Image iconImage;

    /// <summary>
    /// Sets up the rect transform and travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    void Start()
    {

    }

    void Update()
    {

    }
}