using UnityEngine;

public class CharacterVisual : MonoBehaviour
{

    private GameObject model;

    public void SetupCharacterVisual(CharacterVisualData data)
    {
        model = Instantiate(data.model, transform);
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
