using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{

    public List<EnemyBehavior> Behaviors { get; private set; }

    protected EnemyAbility chosenAbility;

    public EnemyCharacter(EnemyData data) : base()
    {
        Stats.HealthMax = data.maxHealth.GetValue();
        Stats.Health = Stats.HealthMax;

        Behaviors = new List<EnemyBehavior>(data.Behaviors.List);
    }

    public override void ResetForTurn()
    {
    }
}