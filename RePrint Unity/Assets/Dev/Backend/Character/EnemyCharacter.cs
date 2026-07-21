using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{

    public List<EnemyBehavior> Behaviors { get; private set; }

    protected EnemyAbility chosenAbility;

    public EnemyAbility ChosenAbility { get => chosenAbility; }

    public EnemyCharacter(EnemyData data) : base()
    {
        Profile = data.Profile;

        Stats.HealthMax = data.maxHealth.GetValue();
        Stats.Health = Stats.HealthMax;

        Behaviors = new List<EnemyBehavior>(data.Behaviors.List);

        Mods = new List<Mod>();
    }

    public virtual void DecideAbility(BattleManager battleManager)
    {
        List<EnemyAbilityWeight> weights = new List<EnemyAbilityWeight>();

        GameValues gameValues = new GameValues
        {
            battleManager = battleManager,
            activator = this,
        };

        foreach (EnemyBehavior behavior in Behaviors)
        {
            if (StatCalculation.DoGameConditionsPass(behavior.Conditions, gameValues))
            {
                weights.AddRange(behavior.AbilityWeights);
                if (behavior.BreakOutIfConditionsAreTrue)
                {
                    break;
                }
            }
        }

        // Add all weights together to basically normalize them.
        int totalWeight = 0;

        foreach (EnemyAbilityWeight abilityWeight in weights)
        {
            totalWeight += abilityWeight.Weight;
        }

        int randomWeight = UnityEngine.Random.Range(0, totalWeight);

        int countedWeight = 0;

        foreach (EnemyAbilityWeight abilityWeight in weights)
        {
            if (randomWeight < countedWeight + abilityWeight.Weight)
            {
                chosenAbility = new EnemyAbility(abilityWeight.AbilityData);
                return;
            }
            countedWeight += abilityWeight.Weight;
        }
    }

    public override void ResetForTurn()
    {
    }
}