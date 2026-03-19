using System.Collections.Generic;

public class Character
{

    public CharacterStats stats;

    public List<Ability> Abilities { get; private set; }

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

        stats.AbilityPointsMax = data.abilityPointsMax.GetValue();
        stats.AbilityPoints = stats.AbilityPointsMax;

        stats.Chain = 0;

        Abilities = new List<Ability>(data.abilities.Length);
        foreach (AbilityData abilityData in data.abilities)
        {
            Abilities.Add(new Ability(abilityData));
        }
    }
}