using System.Collections.Generic;

public enum EnemyIntent
{
    PhysicalDamage,
    Bleed,
    Shock,
    Burn,
    Bio,
    Defensive,
    Buff,
    ChainDamage
}

public class EnemyAbility : Ability
{
    public EnemyIntent Intent { get; private set; }

    public EnemyAbility(EnemyAbilityData data)
    {
        Intent = data.Intent;
        behaviorsTable = new List<List<AbilityBehavior>>(1)
        {
            data.Behaviors.List
        };
    }

    public override int GetAPCost(int overclock = 0)
    {
        return 0;
    }

    public override int GetNumberOfHits(int overclock = 0)
    {
        return 1;
    }

    public override bool CanTargetEnemies(int overclock = 0)
    {
        return false;
    }

    public override bool CanTargetPlayer(int overclock = 0)
    {
        return true;
    }

    public override bool TargetAllEnemies(int overclock = 0)
    {
        return false;
    }
}