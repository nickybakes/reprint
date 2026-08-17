using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private Dictionary<VisualFX, List<VisualFX>> cachedEffects;

    void Awake()
    {
        cachedEffects = new Dictionary<VisualFX, List<VisualFX>>();
    }

    public void CacheEffects(List<VisualFX> effects)
    {
        foreach (VisualFX effect in effects)
        {
            CacheEffect(effect);
        }
    }

    public void CacheEffect(VisualFX effect, int amount = 3)
    {
        if (!cachedEffects.ContainsKey(effect))
        {
            cachedEffects.Add(effect, new List<VisualFX>(amount));
        }

        for (int i = 0; i < amount; i++)
        {
            CacheIndividualEffect(effect, cachedEffects[effect]);
        }
    }

    public VisualFX PlayEffect(VisualFX effectToPlay, Transform spawnTransform, bool stayParented)
    {
        List<VisualFX> effects = cachedEffects[effectToPlay];

        if (effects == null)
        {
            Debug.Log("Playing uncached Visual Effect. Please try to cache this effect at the beginnig of the game instead!");
            CacheEffect(effectToPlay);
            effects = cachedEffects[effectToPlay];
        }

        VisualFX effect = GetCachedEffectObject(effectToPlay, effects);

        effect.gameObject.SetActive(true);

        if (stayParented)
        {
            spawnTransform.gameObject.SetActive(true);
            effect.transform.SetParent(spawnTransform);
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localRotation = Quaternion.identity;
            effect.transform.localScale = Vector3.one;
        }
        else
        {
            effect.transform.position = spawnTransform.position;
            effect.transform.rotation = spawnTransform.rotation;
            effect.transform.localScale = spawnTransform.localScale;
        }

        effect.Spawn();

        return effect;
    }

    /// <summary>
    /// Get a currently inactive Visual Effect from the pool. If none exist, caches a new one.
    /// </summary>
    /// <returns></returns>
    private VisualFX GetCachedEffectObject(VisualFX effect, List<VisualFX> effects)
    {
        foreach (VisualFX effectObject in effects)
        {
            if (!effectObject.gameObject.activeSelf)
            {
                return effectObject;
            }
        }

        return CacheIndividualEffect(effect, effects);
    }

    private VisualFX CacheIndividualEffect(VisualFX effect, List<VisualFX> effects)
    {
        VisualFX effectObject = Instantiate(effect);
        effects.Add(effectObject);
        return effectObject;
    }
}
