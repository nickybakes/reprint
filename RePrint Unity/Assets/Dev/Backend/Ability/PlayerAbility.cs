using System.Collections.Generic;

public class PlayerAbility : Ability
{
    protected List<AbilityRules> rulesTable;

    public PlayerAbility(PlayerAbilityData data)
    {
        baseData = data;

        rulesTable = new List<AbilityRules>(MAX_OVERCLOCK)
        {
            baseData.AbilityRulesOverclock0,
            baseData.AbilityRulesOverclock1,
            baseData.AbilityRulesOverclock2,
            baseData.AbilityRulesOverclock3,
            baseData.AbilityRulesOverclock4
        };

        behaviorsTable = new List<List<AbilityBehavior>>(MAX_OVERCLOCK)
        {
            baseData.AbilityOverclock0Behaviors.List,
            baseData.AbilityOverclock1Behaviors.List,
            baseData.AbilityOverclock2Behaviors.List,
            baseData.AbilityOverclock3Behaviors.List,
            baseData.AbilityOverclock4Behaviors.List
        };
    }

    public override int GetAPCost(int overclock = 0)
    {
        return rulesTable[overclock].APCost;
    }

    public override bool CanTargetEnemies(int overclock = 0)
    {
        return rulesTable[overclock].CanTargetEnemies;
    }

    public override bool CanTargetPlayer(int overclock = 0)
    {
        return rulesTable[overclock].CanTargetPlayer;
    }


    public override bool TargetAllEnemies(int overclock = 0)
    {
        return rulesTable[overclock].TargetAllEnemies;
    }
}