using System;
using UnityEngine;

public class VisualFX : MonoBehaviour
{

    [SerializeField] private float lifetime = 1;

    private float currentTime;

    public void Spawn()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class VisualEffectAndTransform
{
    [SerializeField] private VisualFX visualEffect;
    [SerializeField] private Transform transform;

    public VisualFX VisualEffect { get => visualEffect; }
    public Transform Transform { get => transform; }
}
