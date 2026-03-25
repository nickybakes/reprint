using System;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        image.type = Image.Type.Filled;
    }

    public void UpdateFill(float percentage)
    {
        if (image)
            image.fillAmount = Mathf.Clamp(percentage, 0, 1);
    }
}
