using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreTimingProfile", menuName = "Scriptable Objects/StoreTimingProfile")]
public class StoreTimingProfile : ScriptableObject
{

    // [field: SerializeField] public float TimeBeforePlayerAbilityAnimationSequence { get; private set; } = .25f;

    // [field: SerializeField] public float TimeAfterPlayerAbilityAnimationSequence { get; private set; } = .25f;

    // [SerializeField] private float enemyAbilityTime = .5f;
    // [SerializeField] private float beforePlayerTurnStartTime = .5f;


    public float GetTime(StoreStateChange change)
    {
        return 0;
    }
}
