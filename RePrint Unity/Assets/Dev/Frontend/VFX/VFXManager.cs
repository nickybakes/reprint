using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private Dictionary<VisualEffect, List<VisualEffect>> cachedEffects;

    void Awake()
    {
        cachedEffects = new Dictionary<VisualEffect, List<VisualEffect>>();
    }

    public void CacheEffects(List<VisualEffect> effects)
    {
        foreach (VisualEffect effect in effects)
        {
            CacheEffect(effect);
        }
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

    public VisualEffect PlayEffect(VisualEffect effectToPlay, Transform spawnTransform)
    {
        List<VisualEffect> effects = cachedEffects[effectToPlay];

        if (effects == null)
        {
            Debug.Log("Playing uncached Visual Effect. Please try to cache this effect at the beginnig of the game instead!");
            CacheEffect(effectToPlay);
            effects = cachedEffects[effectToPlay];
        }

        VisualEffect effect = GetCachedEffectObject(effectToPlay, effects);

        effect.gameObject.SetActive(true);
        effect.transform.position = spawnTransform.position;
        effect.transform.rotation = spawnTransform.rotation;
        effect.transform.localScale = spawnTransform.localScale;

        effect.Spawn();

        return effect;
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
