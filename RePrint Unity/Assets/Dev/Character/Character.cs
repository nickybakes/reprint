using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private CharacterStats stats;

    private CharacterVisual visual;

    private List<CharacterActionData> characterActions;

    private bool isPlayerControlled;

    public bool IsPlayerControlled
    {
        get
        {
            return isPlayerControlled;
        }
    }

    public CharacterStats Stats
    {
        get
        {
            return stats;
        }
    }

    public CharacterVisual Visual
    {
        get
        {
            return visual;
        }
    }

    public List<CharacterActionData> CharacterActions
    {
        get
        {
            return characterActions;
        }
    }

    void Awake()
    {
        visual = GetComponent<CharacterVisual>();
    }

    public void SetupCharacter(CharacterData data, bool _isPlayerControlled)
    {
        visual.SetupCharacterVisual(data.visualData);
        isPlayerControlled = _isPlayerControlled;

        stats.healthMax = data.healthMax;

        stats.health = UnityEngine.Random.Range(2, 20);

        characterActions = new List<CharacterActionData>(data.actionDatas);
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


public struct CharacterStats
{
    public int health;
    public int healthMax;

    public int chain;
    public int chainMax;

    public int actionPoints;
    public int actionPointsMax;
}