using UnityEngine;

public class Character : MonoBehaviour
{

    private int health;
    private int healthMax;

    private int chain;
    private int chainMax;

    private int actionPoints;
    private int actionPointsMax;

    private CharacterVisual visual;

    void Awake()
    {
        visual = GetComponent<CharacterVisual>();
    }

    public void SetupCharacter(CharacterData data)
    {
        visual.SetupCharacterVisual(data.visualData);
    }

    public void SetSpawnTransform(Vector3 position, float direction)
    {
        transform.position = position;
        transform.Rotate(Vector3.up * direction);
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
