using UnityEngine;

public class SegmentedMeter : MonoBehaviour
{

    [SerializeField] private GameObject filledPrefab;
    [SerializeField] private GameObject emptyPrefab;

    void Awake()
    {
        filledPrefab.SetActive(false);
        emptyPrefab.SetActive(false);
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Refresh(int amountFilled, int total)
    {
        Clear();

        int j = 0;

        for (int i = 0; i < total; i++)
        {
            if (j < amountFilled)
            {
                GameObject g = Instantiate(filledPrefab, transform);
                g.SetActive(true);
                j++;
            }
            else
            {
                GameObject g = Instantiate(emptyPrefab, transform);
                g.SetActive(true);
            }
        }
    }
}
