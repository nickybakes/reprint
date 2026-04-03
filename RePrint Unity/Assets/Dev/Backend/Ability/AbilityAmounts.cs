using System.Collections.Generic;

public class AbilityAmounts
{

    public float PlayerAmount { get; private set; }

    public Dictionary<Character, float> EnemyAmounts { get; private set; }

    public AbilityAmounts(Character player, Team enemyTeam)
    {
        EnemyAmounts = new Dictionary<Character, float>();
    }


    // public static AbilityAmounts operator +(AbilityAmounts left, AbilityAmounts right)
    // {
    //     AbilityAmounts newAmounts = new AbilityAmounts()
    // }
}