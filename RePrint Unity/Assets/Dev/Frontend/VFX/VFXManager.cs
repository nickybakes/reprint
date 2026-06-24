using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private Dictionary<VisualEffect, List<VisualEffect>> cachedEffects;

    void Awake()
    {
        cachedEffects = new Dictionary<VisualEffect, List<VisualEffect>>();
    }

    public void CacheEffect(VisualEffect effect, int amount = 3)
    {
        if (!cachedEffects.ContainsKey(effect))
        {
            cachedEffects.Add(effect, new List<VisualEffect>(amount));
        }

        for (int i = 0; i < amount; i++)
        {
            CacheIndividualEffect(effect, cachedEffects[effect]);
        }
    }

    public VisualEffect SpawnEffect(VisualEffect effectToSpawn, Transform spawnTransform)
    {
        List<VisualEffect> effects = cachedEffects[effectToSpawn];

        if (effects == null)
        {
            Debug.Log("Spawning uncached Visual Effect. Please try to cache this effect at the beginnig of the game instead!");
            CacheEffect(effectToSpawn);
            effects = cachedEffects[effectToSpawn];
        }

        VisualEffect spawnedVisualEffect = GetCachedEffectObject(effectToSpawn, effects);

        return spawnedVisualEffect;
    }

    /// <summary>
    /// Get a currently inactive Visual Effect from the pool. If none exist, caches a new one.
    /// </summary>
    /// <returns></returns>
    private VisualEffect GetCachedEffectObject(VisualEffect effect, List<VisualEffect> effects)
    {
        foreach (VisualEffect effectObject in effects)
        {
            if (!effectObject.gameObject.activeSelf)
            {
                return effectObject;
            }
        }

        return CacheIndividualEffect(effect, effects);
    }

    private VisualEffect CacheIndividualEffect(VisualEffect effect, List<VisualEffect> effects)
    {
        VisualEffect effectObject = Instantiate(effect);
        effectObject.gameObject.SetActive(false);
        effects.Add(effectObject);
        return effectObject;
    }
}
