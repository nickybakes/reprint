public class IncomingValues
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

    public int GetIncomingValue(ModifierIncomingValue type)
    {
        switch (type)
        {
            case ModifierIncomingValue.Chain:
                return chain;

            case ModifierIncomingValue.NumberOfEnemies:
                return numberOfEnemies;
        }

        return 0;
    }
}