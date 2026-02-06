using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMeter : MonoBehaviour
{

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        image.type = Image.Type.Filled;
    }

    public void UpdateFill(float percentage)
    {
        image.fillAmount = Mathf.Clamp(percentage, 0, 1);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
