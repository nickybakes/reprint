using System.Collections.Generic;

public class Character
{

    public CharacterStats Stats { get; private set; }

    public InGameValues IncomingValues { get; private set; }

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

    public bool IsAlive
    {
        get
        {
            return Stats.Health > 0;
        }
    }

    public Character(CharacterData data)
    {
        Name = data.name;
        IncomingValues = new InGameValues();

        Stats = new CharacterStats();

        Stats.HealthMax = data.maxHealth.GetValue();
        Stats.Health = Stats.HealthMax;

        Stats.AbilityPointsMax = data.abilityPointsMax.GetValue();
        Stats.AbilityPoints = Stats.AbilityPointsMax;

        Stats.Chain = 0;

        Abilities = new List<Ability>(data.abilities.Length);
        foreach (AbilityData abilityData in data.abilities)
        {
            Abilities.Add(new Ability(abilityData));
        }
    }

    public void RefreshIncomingValues(int numberOfEnemies)
    {
        IncomingValues.SetCalculatedChain(this);
        IncomingValues.SetNumberOfEnemies(numberOfEnemies);
    }
}