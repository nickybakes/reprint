using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubDescriptionGroup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI subDescriptionPrefab;
    [SerializeField] private RectTransform backgroundElement;

    [SerializeField] private float spacing;
    [SerializeField] private float bottomMargin;

    private List<TextMeshProUGUI> allDescriptionTexts;

    private float baseBackgroundHeight;

    public void DisplayDescriptions(string mainDescription, List<string> descriptions)
    {
        allDescriptionTexts = new List<TextMeshProUGUI>();

        descriptionText.transform.SetParent(transform);
        allDescriptionTexts.Add(descriptionText);
        descriptionText.text = mainDescription;
        descriptionText.gameObject.SetActive(true);

        for (int i = 0; i < descriptions.Count; i++)
        {
            TextMeshProUGUI newText = Instantiate(subDescriptionPrefab, transform);
            allDescriptionTexts.Add(newText);
            newText.text = descriptions[i];
            newText.gameObject.SetActive(true);
        }

        baseBackgroundHeight = backgroundElement.sizeDelta.y;
    }

    private void Resize()
    {
        float totalHeight = 0;

        for (int i = 0; i < allDescriptionTexts.Count; i++)
        {
            Vector2 sizeDelta = allDescriptionTexts[i].rectTransform.sizeDelta;
            sizeDelta.y = allDescriptionTexts[i].preferredHeight;
            if (i != allDescriptionTexts.Count - 1)
            {
                sizeDelta.y += spacing;
            }
            allDescriptionTexts[i].rectTransform.sizeDelta = sizeDelta;

            totalHeight += sizeDelta.y;
        }

        if (allDescriptionTexts.Count > 0)
        {
            float calculatedBackgroundHeight = baseBackgroundHeight + totalHeight + bottomMargin;
            backgroundElement.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, -((baseBackgroundHeight * .5f) + totalHeight + bottomMargin), calculatedBackgroundHeight);
        }
    }

    void OnEnable()
    {
        Resize();
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
