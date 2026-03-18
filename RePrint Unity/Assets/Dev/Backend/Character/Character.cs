using System.Collections.Generic;

public class Character
{

    public CharacterStats stats;

    public List<CharacterAction> Actions { get; private set; }

    public string Name { get; private set; }

    private int index;

    private bool isPlayerControlled;

    public bool IsPlayerControlled
    {
        get
        {
            return isPlayerControlled;
        }
    }

    public int Index
    {
        get
        {
            return index;
        }
    }

    public Character(CharacterData data)
    {
        Name = data.name;

        stats = new CharacterStats();

        stats.HealthMax = data.maxHealth.GetValue();
        stats.Health = stats.HealthMax;

        stats.ActionPointsMax = data.actionPointsMax.GetValue();
        stats.ActionPoints = stats.ActionPointsMax;

        stats.Chain = 0;

        Actions = new List<CharacterAction>(data.actionDatas.Length);
        foreach (CharacterActionData actionData in data.actionDatas)
        {
            Actions.Add(new CharacterAction(actionData));
        }
    }

    // public void SetupCharacter(CharacterData data, bool _isPlayerControlled, int _index)
    // {
    //     index = _index;
    //     visual.SetupCharacterVisual(data.visualData);
    //     isPlayerControlled = _isPlayerControlled;

    //     stats.healthMax = data.healthMax;
    //     stats.actionPointsMax = data.actionPointsMax;

    //     stats.health = UnityEngine.Random.Range(2, 20);

    //     characterActions = new List<CharacterActionData>(data.actionDatas);
    // }

    // public void SetSpawnTransform(Vector3 position, float direction)
    // {
    //     transform.position = position;
    //     transform.Rotate(Vector3.up * direction);
    // }
}