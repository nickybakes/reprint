using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleTimingProfile", menuName = "Scriptable Objects/BattleTimingProfile")]
public class BattleTimingProfile : ScriptableObject
{

    [SerializeField] private float playerAbilityTime = .5f;
    [SerializeField] private float enemyAbilityTime = .5f;


    public float GetTime(BattleStateChange change)
    {
        // if (change is PlayerDoAbility)
        // {
        //     return playerAbilityTime;
        // }

        if (change is EnemyDoAbility)
        {
            return enemyAbilityTime;
        }

        return 0;
    }
}
