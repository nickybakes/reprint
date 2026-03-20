

public enum InGameValueType
{
    Chain,
    NumberOfEnemies
}


public class InGameValues
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

    public int GetIncomingValue(InGameValueType type)
    {
        switch (type)
        {
            case InGameValueType.Chain:
                return chain;

            case InGameValueType.NumberOfEnemies:
                return numberOfEnemies;
        }

        return 0;
    }
}
