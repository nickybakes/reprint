

public enum GameValueType
{
    Chain,
    NumberOfEnemies
}


public class GameValues
{
    private int chain;

    private int numberOfEnemies;

    public void SetCalculatedChain(Character character)
    {
        // TODO: Alter the chain amount based on character's mod chips.
        chain = character.Stats.Chain;
    }

    public void SetNumberOfEnemies(int _numberOfEnemies)
    {
        numberOfEnemies = _numberOfEnemies;
    }

    public int GetInGameValue(GameValueType type)
    {
        switch (type)
        {
            case GameValueType.Chain:
                return chain;

            case GameValueType.NumberOfEnemies:
                return numberOfEnemies;
        }

        return 0;
    }
}
