using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleTimingProfile", menuName = "Scriptable Objects/BattleTimingProfile")]
public class BattleTimingProfile : ScriptableObject
{

    [field: SerializeField] public float TimeBeforePlayerAbilityAnimationSequence { get; private set; } = .25f;

    [field: SerializeField] public float TimeAfterPlayerAbilityAnimationSequence { get; private set; } = .25f;

    [SerializeField] private float enemyAbilityTime = .5f;
    [SerializeField] private float beforePlayerTurnStartTime = .5f;


    public float GetTime(BattleStateChange change)
    {
        if (change is EnemyDoAbility)
        {
            return enemyAbilityTime;
        }

        if (change is BeforePlayerTurnStart)
        {
            return beforePlayerTurnStartTime;
        }

        return 0;
    }
}
