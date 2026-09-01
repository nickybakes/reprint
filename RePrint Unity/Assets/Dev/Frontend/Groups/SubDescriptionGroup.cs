using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubDescriptionGroup : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI prefab;
    [SerializeField] private RectTransform backgroundElement;

    [SerializeField] private float lineHeight;
    [SerializeField] private float spacing;
    [SerializeField] private float bottomMargin;

    private List<TextMeshProUGUI> texts;

    public void DisplayDescriptions(List<string> descriptions)
    {
        texts = new List<TextMeshProUGUI>();

        for (int i = 0; i < descriptions.Count; i++)
        {
            TextMeshProUGUI newText = Instantiate(prefab, transform);
            texts.Add(newText);
            newText.text = descriptions[i];
            newText.gameObject.SetActive(true);
        }

        Resize();
    }

    private void Resize()
    {
        float totalHeight = 0;

        for (int i = 0; i < texts.Count; i++)
        {
            Vector2 sizeDelta = texts[i].rectTransform.sizeDelta;
            sizeDelta.y = texts[i].textInfo.lineCount * lineHeight;
            if (i != texts.Count - 1)
            {
                sizeDelta.y += spacing;
            }
            texts[i].rectTransform.sizeDelta = sizeDelta;

            totalHeight += sizeDelta.y;
        }

        if (texts.Count > 0)
        {
            Vector2 backgroundSizeDelta = backgroundElement.sizeDelta;
            backgroundSizeDelta.y += totalHeight + bottomMargin;
            backgroundElement.sizeDelta = backgroundSizeDelta;
        }
    }

    void OnEnable()
    {
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
